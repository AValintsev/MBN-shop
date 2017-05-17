using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System.Web.Mvc;
using Nop.Plugin.Misc.QuickOrder.Models;
using Nop.Services.Catalog;
using System.Linq;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;
using Nop.Core.Domain.Customers;
using Nop.Web.Factories;
using Nop.Services.Customers;
using Nop.Web.Models.Checkout;
using Nop.Services.Common;
using Nop.Web.Extensions;
using System;
using Nop.Services.Directory;
using Nop.Core.Domain.Shipping;
using System.Web;
using Nop.Services.Payments;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Common;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.QuickOrder.Controllers
{
    public class QuickOrderController : BasePluginController
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly QOrderSettings _qOrderSettings;

        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;

        private readonly IProductService _productService;
        private readonly OrderSettings _orderSettings;
        private readonly ICheckoutModelFactory _checkoutModelFactory;
        private readonly ICustomerService _customerService;
        private readonly ICountryService _countryService;
        private readonly ShippingSettings _shippingSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderService _orderService;
        private readonly HttpContextBase _httpContext;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IPaymentService _paymentService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly CustomerSettings _customerSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly AddressSettings _addressSettings;





        #endregion

        #region Utilities

        [NonAction]
        protected virtual bool IsMinimumOrderPlacementIntervalValid(Customer customer)
        {
            //prevent 2 orders being placed within an X seconds time frame
            if (_orderSettings.MinimumOrderPlacementInterval == 0)
                return true;

            var lastOrder = _orderService.SearchOrders(storeId: _storeContext.CurrentStore.Id,
                customerId: _workContext.CurrentCustomer.Id, pageSize: 1)
                .FirstOrDefault();
            if (lastOrder == null)
                return true;

            var interval = DateTime.UtcNow - lastOrder.CreatedOnUtc;
            return interval.TotalSeconds > _orderSettings.MinimumOrderPlacementInterval;
        }

        #endregion
        
        #region Ctor
        public QuickOrderController(
            IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            IStoreContext storeContext,
            IProductService productService,
            QOrderSettings qOrderSettings,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            OrderSettings orderSettings,
            ICheckoutModelFactory checkoutModelFactory,
            ICustomerService customerService,
            ICountryService countryService,
            ShippingSettings shippingSettings,
            IOrderService orderService,
            HttpContextBase httpContext,
            IOrderProcessingService orderProcessingService,
            IPaymentService paymentService,
            CustomerSettings customerSettings,
            IShoppingCartService shoppingCartService,
            PaymentSettings paymentSettings,
            AddressSettings addressSettings,
            IGenericAttributeService genericAttributeService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._productService = productService;
            this._qOrderSettings = qOrderSettings;

            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;

            this._orderSettings = orderSettings;
            this._checkoutModelFactory = checkoutModelFactory;
            this._customerService = customerService;
            this._countryService = countryService;
            this._shippingSettings = shippingSettings;
            this._genericAttributeService = genericAttributeService;
            this._orderService = orderService;
            this._httpContext = httpContext;
            this._orderProcessingService = orderProcessingService;
            this._paymentService = paymentService;
            this._shoppingCartService = shoppingCartService;
            this._customerSettings = customerSettings;
            this._paymentSettings = paymentSettings;
            this._addressSettings = addressSettings;




        }

        #endregion 

        public ActionResult Form()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var qOrderSettings = _settingService.LoadSetting<QOrderSettings>(storeScope);

            if (!qOrderSettings.Enabled)
                return DONE("");

            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();
            if (!cart.Any())
              return DONE("");

            bool downloadableProductsRequireRegistration =
                _customerSettings.RequireRegistrationForDownloadableProducts && cart.Any(sci => sci.Product.IsDownload);

            if (_workContext.CurrentCustomer.IsGuest()
                && (!_orderSettings.AnonymousCheckoutAllowed
                    || downloadableProductsRequireRegistration)
                )
                return DONE("");

            //if we have only "button" payment methods available(displayed onthe shopping cart page, not during checkout),
            //then we should allow standard checkout
            //all payment methods(do not filter by country here as it could be not specified yet)
            var paymentMethods = _paymentService
                .LoadActivePaymentMethods(_workContext.CurrentCustomer, _storeContext.CurrentStore.Id)
                .Where(pm => !pm.HidePaymentMethod(cart))
                .ToList();
            //payment methods displayed during checkout (not with "Button" type)
            var nonButtonPaymentMethods = paymentMethods
                .Where(pm => pm.PaymentMethodType != PaymentMethodType.Button)
                .ToList();
            //"button" payment methods(*displayed on the shopping cart page)
            var buttonPaymentMethods = paymentMethods
                .Where(pm => pm.PaymentMethodType == PaymentMethodType.Button)
                .ToList();
            if (!nonButtonPaymentMethods.Any() && buttonPaymentMethods.Any())
                return DONE("");

            //reset checkout data
            _customerService.ResetCheckoutData(_workContext.CurrentCustomer, _storeContext.CurrentStore.Id);

            //validation (cart)
            var checkoutAttributesXml = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.CheckoutAttributes, _genericAttributeService, _storeContext.CurrentStore.Id);
            var scWarnings = _shoppingCartService.GetShoppingCartWarnings(cart, checkoutAttributesXml, true);
            if (scWarnings.Any())
                return DONE("");
            //validation (each shopping cart item)
            foreach (ShoppingCartItem sci in cart)
            {
                var sciWarnings = _shoppingCartService.GetShoppingCartItemWarnings(_workContext.CurrentCustomer,
                    sci.ShoppingCartType,
                    sci.Product,
                    sci.StoreId,
                    sci.AttributesXml,
                    sci.CustomerEnteredPrice,
                    sci.RentalStartDateUtc,
                    sci.RentalEndDateUtc,
                    sci.Quantity,
                    false);
                if (sciWarnings.Any())
                    return DONE("");
            }

            var model = new QOrderModel
            {
                NameEnable = _qOrderSettings.NameEnabled,
                EmailEnable = _qOrderSettings.NameEnabled,
                PhoneEnable = _qOrderSettings.PhoneEnabled
            };
            return View("~/Plugins/Misc.QuickOrder/Views/Form.cshtml", model);
        }

        [HttpPost]
        public void Form(QOrderModel order)
        {
            if (ModelState.IsValid)
            {
                int id = Process(order);
                Response.RedirectToRoute("CheckoutCompleted", new { orderId = id });

            }
            else
            {
                Form();
            }
        }
        [AdminAuthorize]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var qOrderSettings = _settingService.LoadSetting<QOrderSettings>(storeScope);

            var viewModel = new ConfigurationModel
            {
                Enabled = qOrderSettings.Enabled,
                EmailAddressEnabled = qOrderSettings.EmailAddressEnabled,
                EmailAddressRequired = qOrderSettings.EmailAddressRequired,
                NameEnabled = qOrderSettings.NameEnabled,
                NameRequired = qOrderSettings.NameRequired,
                PhoneEnabled = qOrderSettings.PhoneEnabled,
                PhoneRequired = qOrderSettings.PhoneRequired,
                WidgetZone = qOrderSettings.WidgetZone                
            };

            viewModel.AvailableWidgets.Add(new SelectListItem { Text = "order_summary_cart_footer", Value = "order_summary_cart_footer" });
            viewModel.AvailableWidgets.Add(new SelectListItem { Text = "order_summary_content_after", Value = "order_summary_content_after" });
            viewModel.AvailableWidgets.Add(new SelectListItem { Text = "order_summary_content_before", Value = "order_summary_content_before" });
            viewModel.AvailableWidgets.Add(new SelectListItem { Text = "order_summary_content_custom", Value = "order_summary_content_custom" });

            return View("~/Plugins/Misc.QuickOrder/Views/Configure.cshtml", viewModel);
        }

        
		[HttpPost]
		[ChildActionOnly]
		[FormValueRequired("save")]
		public ActionResult Configure(ConfigurationModel viewModel)
		{
            if (!ModelState.IsValid)
            {
                return Configure();
            }

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var qOrderSettings = _settingService.LoadSetting<QOrderSettings>(storeScope);

            //save settings
            qOrderSettings.Enabled = viewModel.Enabled;
            qOrderSettings.NameEnabled = viewModel.NameEnabled;
            qOrderSettings.NameRequired = viewModel.NameRequired;
            qOrderSettings.EmailAddressEnabled = viewModel.EmailAddressEnabled;
            qOrderSettings.EmailAddressRequired = viewModel.EmailAddressRequired;
            qOrderSettings.PhoneEnabled = viewModel.PhoneEnabled;
            qOrderSettings.PhoneRequired = viewModel.PhoneRequired;
            qOrderSettings.WidgetZone = viewModel.WidgetZone;

			_settingService.SaveSetting(qOrderSettings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

        public ActionResult DONE(string message)
        {
            ViewBag.message = message;
            return View("~/Plugins/Misc.QuickOrder/Views/DONE.cshtml");
        }

        private int Process(QOrderModel form)
        {
            try { 
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();

            if (!cart.Any())
              DONE("Your cart is empty");


            if (_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed)
               DONE("");


            var billingAddressModel = _checkoutModelFactory.PrepareBillingAddressModel(cart, prePopulateNewAddressWithCustomerFields: true);


            int billingAddressId = billingAddressModel.ExistingAddresses.Count() > 0 ? 
                    billingAddressModel.ExistingAddresses.FirstOrDefault().Id : 0;
                

            if (billingAddressId > 0)
            {
                //existing address
                var address = _workContext.CurrentCustomer.Addresses.FirstOrDefault(a => a.Id == billingAddressId);
                if (address == null)
                    throw new Exception("Address can't be loaded");

                _workContext.CurrentCustomer.BillingAddress = address;
                _customerService.UpdateCustomer(_workContext.CurrentCustomer);
            }
            else
            {
                //new address
                var model = new CheckoutBillingAddressModel();

                    //?? My Code 

                    //TryUpdateModel(model.NewAddress, "BillingNewAddress");
                    model.NewAddress.Email = form.CustomerEmail;
                    model.NewAddress.PhoneNumber = form.CustomerPhone;
                    model.NewAddress.FirstName = form.CustomerName;
                    //?? // My Code

                    ////custom address attributes
                    var customAttributes = "";
                    //form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);  // Unnecessary 
                //var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
                //foreach (var error in customAttributeWarnings)
                //{
                //    ModelState.AddModelError("", error);
                //}

                //validate model
             //   TryValidateModel(model.NewAddress);
                if (!ModelState.IsValid)
                {
                    //model is not valid. redisplay the form with errors
                     billingAddressModel = _checkoutModelFactory.PrepareBillingAddressModel(cart,
                        selectedCountryId: model.NewAddress.CountryId,
                        overrideAttributesXml: customAttributes);
                    billingAddressModel.NewAddressPreselected = true;

                        //?? My Code 
                        throw new Exception("WRONG!!! wrong billing address");
                        //?? // My Code 

                        //return Json(new
                        //{
                        //    update_section = new UpdateSectionJsonModel
                        //    {
                        //        name = "billing",
                        //        html = this.RenderPartialViewToString("OpcBillingAddress", billingAddressModel)
                        //    },
                        //    wrong_billing_address = true,
                        //});
                    }

                    //try to find an address with the same values (don't duplicate records)
                    var address = _workContext.CurrentCustomer.Addresses.ToList().FindAddress(
                    model.NewAddress.FirstName, model.NewAddress.LastName, model.NewAddress.PhoneNumber,
                    model.NewAddress.Email, model.NewAddress.FaxNumber, model.NewAddress.Company,
                    model.NewAddress.Address1, model.NewAddress.Address2, model.NewAddress.City,
                    model.NewAddress.StateProvinceId, model.NewAddress.ZipPostalCode,
                    model.NewAddress.CountryId, customAttributes);
                if (address == null)
                {
                    //address is not found. let's create a new one
                    address = model.NewAddress.ToEntity();
                    address.CustomAttributes = customAttributes;
                    address.CreatedOnUtc = DateTime.UtcNow;
                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;
                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;
                    if (address.CountryId.HasValue && address.CountryId.Value > 0)
                    {
                        address.Country = _countryService.GetCountryById(address.CountryId.Value);
                    }
                    _workContext.CurrentCustomer.Addresses.Add(address);
                }
                _workContext.CurrentCustomer.BillingAddress = address;
                _customerService.UpdateCustomer(_workContext.CurrentCustomer);
            }

                //load next step
                //return OpcLoadStepAfterShippingMethod(cart);

                ////payment is not required
                //_genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer,
                //    SystemCustomerAttributeNames.SelectedPaymentMethod, null, _storeContext.CurrentStore.Id);

                //?? 

                LoadShippingAddress();

                // ?? PAYMENT METHOD

                ////load payment method
                //var paymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute<string>(
                //    SystemCustomerAttributeNames.SelectedPaymentMethod,
                //    _genericAttributeService, _storeContext.CurrentStore.Id);
                //var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(paymentMethodSystemName);
                //if (paymentMethod == null)
                //    //return RedirectToRoute("CheckoutPaymentMethod");
                //    throw new Exception("Where iS PAYMENTMETHOD");

                ////Check whether payment info should be skipped
                //if (paymentMethod.SkipPaymentInfo ||
                //    (paymentMethod.PaymentMethodType == PaymentMethodType.Redirection && _paymentSettings.SkipPaymentInfoStepForRedirectionPaymentMethods))
                //{
                //    //skip payment info page
                //    var paymentInfo = new ProcessPaymentRequest();
                //    //session save
                //    _httpContext.Session["OrderPaymentInfo"] = paymentInfo;

                //    return RedirectToRoute("CheckoutConfirm");
                //}


                bool isPaymentWorkflowRequired = _orderProcessingService.IsPaymentWorkflowRequired(cart, false);
                if (isPaymentWorkflowRequired)
                {
                    //filter by country
                    int filterByCountryId = 0;
                    if (_addressSettings.CountryEnabled &&
                        _workContext.CurrentCustomer.BillingAddress != null &&
                        _workContext.CurrentCustomer.BillingAddress.Country != null)
                    {
                        filterByCountryId = _workContext.CurrentCustomer.BillingAddress.Country.Id;
                    }

                    //payment is required
                    var paymentMethodModel = _checkoutModelFactory.PreparePaymentMethodModel(cart, filterByCountryId);

                    //if (_paymentSettings.BypassPaymentMethodSelectionIfOnlyOne &&
                    //    paymentMethodModel.PaymentMethods.Count == 1 && !paymentMethodModel.DisplayRewardPoints)
                    //{
                    //if we have only one payment method and reward points are disabled or the current customer doesn't have any reward points
                    //so customer doesn't have to choose a payment method

                    var selectedPaymentMethodSystemName = paymentMethodModel.PaymentMethods[0].PaymentMethodSystemName;
                    _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                        SystemCustomerAttributeNames.SelectedPaymentMethod,
                        selectedPaymentMethodSystemName, _storeContext.CurrentStore.Id);
                

                    // ?? FOR WHAT????

                        //var paymentMethodInst = _paymentService.LoadPaymentMethodBySystemName(selectedPaymentMethodSystemName);
                        //if (paymentMethodInst == null ||
                        //    !paymentMethodInst.IsPaymentMethodActive(_paymentSettings) ||
                        //    !_pluginFinder.AuthenticateStore(paymentMethodInst.PluginDescriptor, _storeContext.CurrentStore.Id) ||
                        //    !_pluginFinder.AuthorizedForUser(paymentMethodInst.PluginDescriptor, _workContext.CurrentCustomer))
                        //    throw new Exception("Selected payment method can't be parsed");

                        //return OpcLoadStepAfterPaymentMethod(paymentMethodInst, cart);
                 }



                    // ?? ORDER CONFITM

                    try
                {

                    if (_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed)
                        throw new Exception("Anonymous checkout is not allowed");

                    //prevent 2 orders being placed within an X seconds time frame
                    if (!IsMinimumOrderPlacementIntervalValid(_workContext.CurrentCustomer))
                        throw new Exception(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));

                    //place order
                    //var processPaymentRequest = _httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;
                    //if (processPaymentRequest == null)
                    //{
                    //    //Check whether payment workflow is required
                    //    if (_orderProcessingService.IsPaymentWorkflowRequired(cart))
                    //    {
                    //        throw new Exception("Payment information is not entered");
                    //    }
                    //    else
                           var processPaymentRequest = new ProcessPaymentRequest();
                    //}

                    processPaymentRequest.StoreId = _storeContext.CurrentStore.Id;
                    processPaymentRequest.CustomerId = _workContext.CurrentCustomer.Id;
                    processPaymentRequest.PaymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute<string>(
                        SystemCustomerAttributeNames.SelectedPaymentMethod,
                        _genericAttributeService, _storeContext.CurrentStore.Id);
                    var placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest);
                    if (placeOrderResult.Success)
                    {
                        _httpContext.Session["OrderPaymentInfo"] = null;
                        var postProcessPaymentRequest = new PostProcessPaymentRequest
                        {
                            Order = placeOrderResult.PlacedOrder
                        };


                        var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(placeOrderResult.PlacedOrder.PaymentMethodSystemName);
                        //if (paymentMethod == null)
                            //payment method could be null if order total is 0
                            //success
                           // return Json(new { success = 1 });

                        //if (paymentMethod.PaymentMethodType == PaymentMethodType.Redirection)
                        //{
                        //    //Redirection will not work because it's AJAX request.
                        //    //That's why we don't process it here (we redirect a user to another page where he'll be redirected)

                        //    //redirect
                        //    return Json(new
                        //    {
                        //        redirect = string.Format("{0}checkout/OpcCompleteRedirectionPayment", _webHelper.GetStoreLocation())
                        //    });
                        //}

                        _paymentService.PostProcessPayment(postProcessPaymentRequest);
                        //success
                        //return Json(new { success = 1 });

                       return placeOrderResult.PlacedOrder.Id;
                    }

                    //error
                    var confirmOrderModel = new CheckoutConfirmModel();
                    foreach (var error in placeOrderResult.Errors)
                        confirmOrderModel.Warnings.Add(error);

                    throw new Exception("ERROR HAPENED with PAYMENT");

                    //return Json(new
                    //{
                    //    update_section = new UpdateSectionJsonModel
                    //    {
                    //        name = "confirm-order",
                    //        html = this.RenderPartialViewToString("OpcConfirmOrder", confirmOrderModel)
                    //    },
                    //    goto_section = "confirm_order"
                    //});
                }
                catch (Exception exc)
                {
                   // _logger.Warning(exc.Message, exc, _workContext.CurrentCustomer);
                    // return Json(new { error = 1, message = exc.Message });

                    throw exc;
                }
            }
            catch (Exception exc)
            {
                //_logger.Warning(exc.Message, exc, _workContext.CurrentCustomer);
                //return Json(new { error = 1, message = exc.Message
                //});

                throw exc;
            }

        }

        private void LoadShippingAddress()
        {
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
               .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
               .LimitPerStore(_storeContext.CurrentStore.Id)
               .ToList();

            if (cart.RequiresShipping())
            {
                //shipping is required
                // ?? SHIPPING IS THE SAME AS BILLING

                var model = new CheckoutBillingAddressModel();
                TryUpdateModel(model);

                //ship to the same address
                _workContext.CurrentCustomer.ShippingAddress = _workContext.CurrentCustomer.BillingAddress;
                _customerService.UpdateCustomer(_workContext.CurrentCustomer);
                //reset selected shipping method (in case if "pick up in store" was selected)
                _genericAttributeService.SaveAttribute<ShippingOption>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, _storeContext.CurrentStore.Id);
                _genericAttributeService.SaveAttribute<PickupPoint>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPickupPoint, null, _storeContext.CurrentStore.Id);
                //limitation - "Ship to the same address" doesn't properly work in "pick up in store only" case (when no shipping plugins are available) 
                //return OpcLoadStepAfterShippingAddress(cart);
            }
            else
            {
                //shipping is not required
                _workContext.CurrentCustomer.ShippingAddress = null;
                _customerService.UpdateCustomer(_workContext.CurrentCustomer);

                _genericAttributeService.SaveAttribute<ShippingOption>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, _storeContext.CurrentStore.Id);
            }
        }
    }
}