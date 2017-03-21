using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain;
using Nop.Core.Domain.Affiliates;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Logging;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Polls;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Tasks;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Topics;
using Nop.Core.Domain.Vendors;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;

namespace Nop.Services.Installation
{
	public partial class CodeFirstInstallationServiceMBN : IInstallationService
	{
		#region Fields
				
		private readonly IRepository<Store> _storeRepository;
		private readonly IRepository<MeasureDimension> _measureDimensionRepository;
		private readonly IRepository<MeasureWeight> _measureWeightRepository;
		private readonly IRepository<TaxCategory> _taxCategoryRepository;
		private readonly IRepository<Language> _languageRepository;
		private readonly IRepository<Currency> _currencyRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<CustomerPassword> _customerPasswordRepository;
		private readonly IRepository<CustomerRole> _customerRoleRepository;
		private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;
		private readonly IRepository<CheckoutAttribute> _checkoutAttributeRepository;
		private readonly IRepository<ProductAttribute> _productAttributeRepository;
		private readonly IRepository<Category> _categoryRepository;
		private readonly IRepository<Manufacturer> _manufacturerRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<UrlRecord> _urlRecordRepository;
		private readonly IRepository<RelatedProduct> _relatedProductRepository;
		private readonly IRepository<EmailAccount> _emailAccountRepository;
		private readonly IRepository<MessageTemplate> _messageTemplateRepository;
		private readonly IRepository<ForumGroup> _forumGroupRepository;
		private readonly IRepository<Forum> _forumRepository;
		private readonly IRepository<Country> _countryRepository;
		private readonly IRepository<StateProvince> _stateProvinceRepository;
		private readonly IRepository<Discount> _discountRepository;
		private readonly IRepository<BlogPost> _blogPostRepository;
		private readonly IRepository<Topic> _topicRepository;
		private readonly IRepository<NewsItem> _newsItemRepository;
		private readonly IRepository<Poll> _pollRepository;
		private readonly IRepository<ShippingMethod> _shippingMethodRepository;
		private readonly IRepository<DeliveryDate> _deliveryDateRepository;
		private readonly IRepository<ProductAvailabilityRange> _productAvailabilityRangeRepository;
		private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
		private readonly IRepository<ActivityLog> _activityLogRepository;
		private readonly IRepository<ProductTag> _productTagRepository;
		private readonly IRepository<ProductTemplate> _productTemplateRepository;
		private readonly IRepository<CategoryTemplate> _categoryTemplateRepository;
		private readonly IRepository<ManufacturerTemplate> _manufacturerTemplateRepository;
		private readonly IRepository<TopicTemplate> _topicTemplateRepository;
		private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
		private readonly IRepository<ReturnRequestReason> _returnRequestReasonRepository;
		private readonly IRepository<ReturnRequestAction> _returnRequestActionRepository;
		private readonly IRepository<Address> _addressRepository;
		private readonly IRepository<Warehouse> _warehouseRepository;
		private readonly IRepository<Vendor> _vendorRepository;
		private readonly IRepository<Affiliate> _affiliateRepository;
		private readonly IRepository<Order> _orderRepository;
		private readonly IRepository<OrderItem> _orderItemRepository;
		private readonly IRepository<OrderNote> _orderNoteRepository;
		private readonly IRepository<GiftCard> _giftCardRepository;
		private readonly IRepository<Shipment> _shipmentRepository;
		private readonly IRepository<SearchTerm> _searchTermRepository;
		private readonly IRepository<ShipmentItem> _shipmentItemRepository;
		private readonly IRepository<StockQuantityHistory> _stockQuantityHistoryRepository;
		private readonly IGenericAttributeService _genericAttributeService;
		private readonly IWebHelper _webHelper;

		#endregion

		#region Ctor

		public CodeFirstInstallationServiceMBN(IRepository<Store> storeRepository,
			IRepository<MeasureDimension> measureDimensionRepository,
			IRepository<MeasureWeight> measureWeightRepository,
			IRepository<TaxCategory> taxCategoryRepository,
			IRepository<Language> languageRepository,
			IRepository<Currency> currencyRepository,
			IRepository<Customer> customerRepository,
			IRepository<CustomerPassword> customerPasswordRepository,
			IRepository<CustomerRole> customerRoleRepository,
			IRepository<SpecificationAttribute> specificationAttributeRepository,
			IRepository<CheckoutAttribute> checkoutAttributeRepository,
			IRepository<ProductAttribute> productAttributeRepository,
			IRepository<Category> categoryRepository,
			IRepository<Manufacturer> manufacturerRepository,
			IRepository<Product> productRepository,
			IRepository<UrlRecord> urlRecordRepository,
			IRepository<RelatedProduct> relatedProductRepository,
			IRepository<EmailAccount> emailAccountRepository,
			IRepository<MessageTemplate> messageTemplateRepository,
			IRepository<ForumGroup> forumGroupRepository,
			IRepository<Forum> forumRepository,
			IRepository<Country> countryRepository,
			IRepository<StateProvince> stateProvinceRepository,
			IRepository<Discount> discountRepository,
			IRepository<BlogPost> blogPostRepository,
			IRepository<Topic> topicRepository,
			IRepository<NewsItem> newsItemRepository,
			IRepository<Poll> pollRepository,
			IRepository<ShippingMethod> shippingMethodRepository,
			IRepository<DeliveryDate> deliveryDateRepository,
			IRepository<ProductAvailabilityRange> productAvailabilityRangeRepository,
			IRepository<ActivityLogType> activityLogTypeRepository,
			IRepository<ActivityLog> activityLogRepository,
			IRepository<ProductTag> productTagRepository,
			IRepository<ProductTemplate> productTemplateRepository,
			IRepository<CategoryTemplate> categoryTemplateRepository,
			IRepository<ManufacturerTemplate> manufacturerTemplateRepository,
			IRepository<TopicTemplate> topicTemplateRepository,
			IRepository<ScheduleTask> scheduleTaskRepository,
			IRepository<ReturnRequestReason> returnRequestReasonRepository,
			IRepository<ReturnRequestAction> returnRequestActionRepository,
			IRepository<Address> addressRepository,
			IRepository<Warehouse> warehouseRepository,
			IRepository<Vendor> vendorRepository,
			IRepository<Affiliate> affiliateRepository,
			IRepository<Order> orderRepository,
			IRepository<OrderItem> orderItemRepository,
			IRepository<OrderNote> orderNoteRepository,
			IRepository<GiftCard> giftCardRepository,
			IRepository<Shipment> shipmentRepository,
			IRepository<ShipmentItem> shipmentItemRepository,
			IRepository<SearchTerm> searchTermRepository,
			IRepository<StockQuantityHistory> stockQuantityHistoryRepository,
			IGenericAttributeService genericAttributeService,
			IWebHelper webHelper)
		{
			this._storeRepository = storeRepository;
			this._measureDimensionRepository = measureDimensionRepository;
			this._measureWeightRepository = measureWeightRepository;
			this._taxCategoryRepository = taxCategoryRepository;
			this._languageRepository = languageRepository;
			this._currencyRepository = currencyRepository;
			this._customerRepository = customerRepository;
			this._customerPasswordRepository = customerPasswordRepository;
			this._customerRoleRepository = customerRoleRepository;
			this._specificationAttributeRepository = specificationAttributeRepository;
			this._checkoutAttributeRepository = checkoutAttributeRepository;
			this._productAttributeRepository = productAttributeRepository;
			this._categoryRepository = categoryRepository;
			this._manufacturerRepository = manufacturerRepository;
			this._productRepository = productRepository;
			this._urlRecordRepository = urlRecordRepository;
			this._relatedProductRepository = relatedProductRepository;
			this._emailAccountRepository = emailAccountRepository;
			this._messageTemplateRepository = messageTemplateRepository;
			this._forumGroupRepository = forumGroupRepository;
			this._forumRepository = forumRepository;
			this._countryRepository = countryRepository;
			this._stateProvinceRepository = stateProvinceRepository;
			this._discountRepository = discountRepository;
			this._blogPostRepository = blogPostRepository;
			this._topicRepository = topicRepository;
			this._newsItemRepository = newsItemRepository;
			this._pollRepository = pollRepository;
			this._shippingMethodRepository = shippingMethodRepository;
			this._deliveryDateRepository = deliveryDateRepository;
			this._productAvailabilityRangeRepository = productAvailabilityRangeRepository;
			this._activityLogTypeRepository = activityLogTypeRepository;
			this._activityLogRepository = activityLogRepository;
			this._productTagRepository = productTagRepository;
			this._productTemplateRepository = productTemplateRepository;
			this._categoryTemplateRepository = categoryTemplateRepository;
			this._manufacturerTemplateRepository = manufacturerTemplateRepository;
			this._topicTemplateRepository = topicTemplateRepository;
			this._scheduleTaskRepository = scheduleTaskRepository;
			this._returnRequestReasonRepository = returnRequestReasonRepository;
			this._returnRequestActionRepository = returnRequestActionRepository;
			this._addressRepository = addressRepository;
			this._warehouseRepository = warehouseRepository;
			this._vendorRepository = vendorRepository;
			this._affiliateRepository = affiliateRepository;
			this._orderRepository = orderRepository;
			this._orderItemRepository = orderItemRepository;
			this._orderNoteRepository = orderNoteRepository;
			this._giftCardRepository = giftCardRepository;
			this._shipmentRepository = shipmentRepository;
			this._shipmentItemRepository = shipmentItemRepository;
			this._searchTermRepository = searchTermRepository;
			this._stockQuantityHistoryRepository = stockQuantityHistoryRepository;
			this._genericAttributeService = genericAttributeService;
			this._webHelper = webHelper;
		}

		#endregion

		#region Utilities

		protected virtual void InstallStores()
		{
			//var storeUrl = "http://www.mbnpro.com.ua/";
			var storeUrl = _webHelper.GetStoreLocation(false);
			var stores = new List<Store>
			{
				new Store
				{
					Name = "MBN Pro",
					Url = storeUrl,
					SslEnabled = false,
					Hosts = "mbnpro.com.ua,www.mbnpro.com.ua",
					DisplayOrder = 1,
                    //should we set some default company info?
                    CompanyName = "MBN Pro",
					CompanyAddress = "03022, г. Киев, ул. Михаила Максимовича, 8 ",
					CompanyPhoneNumber = "+380634203770",
					CompanyVat = null,
				},
			};

			_storeRepository.Insert(stores);
		}

		protected virtual void InstallMeasures()
		{
			var measureDimensions = new List<MeasureDimension>
			{
				new MeasureDimension
				{
					Name = "meter(s)",
					SystemKeyword = "meters",
					Ratio = 0.0254M,
					DisplayOrder = 3,
				},
				new MeasureDimension
				{
					Name = "millimetre(s)",
					SystemKeyword = "millimetres",
					Ratio = 25.4M,
					DisplayOrder = 4,
				}
			};

			_measureDimensionRepository.Insert(measureDimensions);

			var measureWeights = new List<MeasureWeight>
			{
				new MeasureWeight
				{
					Name = "kg(s)",
					SystemKeyword = "kg",
					Ratio = 0.45359237M,
					DisplayOrder = 3,
				},
				new MeasureWeight
				{
					Name = "gram(s)",
					SystemKeyword = "grams",
					Ratio = 453.59237M,
					DisplayOrder = 4,
				}
			};

			_measureWeightRepository.Insert(measureWeights);
		}

		protected virtual void InstallTaxCategories()
		{
			//var taxCategories = new List<TaxCategory>
			//                            {
			//                                new TaxCategory
			//                                    {
			//                                        Name = "Electronics & Software",
			//                                        DisplayOrder = 5,
			//                                    },                                   
			//                            };
			//_taxCategoryRepository.Insert(taxCategories);

		}


		private Language languageRu;
		private Language languageUa;
		protected virtual void InstallLanguages()
		{
			languageRu = new Language
			{
				Name = "Russian",
				LanguageCulture = "ru-RU",
				UniqueSeoCode = "ru",
				FlagImageFileName = "ru.png",
				Published = true,
				DisplayOrder = 1
			};
			languageUa = new Language
			{
				Name = "Ukrainian",
				LanguageCulture = "uk-UA",
				UniqueSeoCode = "ua",
				FlagImageFileName = "ua.png",
				Published = true,
				DisplayOrder = 2
			};
			_languageRepository.Insert(languageRu);
			_languageRepository.Insert(languageUa);
		}

		protected virtual void InstallLocaleResources()
		{
			//'English' language
			//var language = _languageRepository.Table.Single(l => l.Name == "English");

			//Russian language
			var languageRu = _languageRepository.Table.Single(l => l.Name == "Russian");

			//save resources
			foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/App_Data/Localization/"), "ru_language_pack.xml", SearchOption.TopDirectoryOnly))
			{
				var localesXml = File.ReadAllText(filePath);
				var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
				localizationService.ImportResourcesFromXml(languageRu, localesXml);
			}

			//Ukrainian language
			var languageUa = _languageRepository.Table.Single(l => l.Name == "Ukrainian");

			//save resources
			foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/App_Data/Localization/"), "ua_language_pack.xml", SearchOption.TopDirectoryOnly))
			{
				var localesXml = File.ReadAllText(filePath);
				var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
				localizationService.ImportResourcesFromXml(languageUa, localesXml);
			}
		}

		protected virtual void InstallCurrencies()
		{
			var currencies = new List<Currency>
			{
				new Currency
				{
					Name = "US Dollar",
					CurrencyCode = "USD",
					Rate = 1,
					DisplayLocale = "en-US",
					CustomFormatting = "",
					Published = true,
					DisplayOrder = 1,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
				new Currency
				{
					Name = "Ukrainian Hrivnia",
					CurrencyCode = "UAH",
					Rate = 1.0M,
					DisplayLocale = "ru-RU",
					CustomFormatting = "",
					Published = true,
					DisplayOrder = 2,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
				},
			};
			_currencyRepository.Insert(currencies);
		}

		protected virtual void InstallCountriesAndStates()
		{
			var ukr = new Country
			{
				Name = "Ukraine",
				AllowsBilling = true,
				AllowsShipping = true,
				TwoLetterIsoCode = "UA",
				ThreeLetterIsoCode = "UKR",
				NumericIsoCode = 804,
				SubjectToVat = false,
				DisplayOrder = 1,
				Published = true
			};
			#region Ukrainian provinces
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АВ (Винницкая область)",
				Abbreviation = "АВ",
				Published = true,
				DisplayOrder = 2
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АС (Волынская область)",
				Abbreviation = "АС",
				Published = true,
				DisplayOrder = 3
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АE (Днепропетровская область)",
				Abbreviation = "АE",
				Published = true,
				DisplayOrder = 4
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АН (Донецкая область)",
				Abbreviation = "АН",
				Published = true,
				DisplayOrder = 5
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АМ (Житомирская область)",
				Abbreviation = "АМ",
				Published = true,
				DisplayOrder = 6
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АО (Закарпатская область)",
				Abbreviation = "АО",
				Published = true,
				DisplayOrder = 7
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АР (Запорізька область)",
				Abbreviation = "АР",
				Published = true,
				DisplayOrder = 8
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АТ (Ивано-Франковская область)",
				Abbreviation = "АТ",
				Published = true,
				DisplayOrder = 9
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АА (Киевская область)",
				Abbreviation = "АА",
				Published = true,
				DisplayOrder = 10
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВА (Кировоградская область)",
				Abbreviation = "ВА",
				Published = true,
				DisplayOrder = 11
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВВ (Луганская область)",
				Abbreviation = "ВВ",
				Published = true,
				DisplayOrder = 12
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВВ (Луганская область)",
				Abbreviation = "ВВ",
				Published = true,
				DisplayOrder = 12
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВС (Львовская область)",
				Abbreviation = "ВС",
				Published = true,
				DisplayOrder = 13
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВЕ (Николаевская область)",
				Abbreviation = "ВЕ",
				Published = true,
				DisplayOrder = 14
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВН (Одесская область)",
				Abbreviation = "ВН",
				Published = true,
				DisplayOrder = 15
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВI (Полтавская область)",
				Abbreviation = "ВI",
				Published = true,
				DisplayOrder = 16
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВК (Ровненская область)",
				Abbreviation = "ВК",
				Published = true,
				DisplayOrder = 17
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВМ (Сумская область)",
				Abbreviation = "ВМ",
				Published = true,
				DisplayOrder = 18
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВО (Тернопільська область)",
				Abbreviation = "ВО",
				Published = true,
				DisplayOrder = 19
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "АХ (Харковская область)",
				Abbreviation = "АХ",
				Published = true,
				DisplayOrder = 20
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВТ (Херсонская область)",
				Abbreviation = "ВТ",
				Published = true,
				DisplayOrder = 21
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "ВХ (Хмельницкая область)",
				Abbreviation = "ВХ",
				Published = true,
				DisplayOrder = 22
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "СА (Черкасская область)",
				Abbreviation = "СА",
				Published = true,
				DisplayOrder = 23
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "СВ (Черниговская область)",
				Abbreviation = "СВ",
				Published = true,
				DisplayOrder = 24
			});
			ukr.StateProvinces.Add(new StateProvince
			{
				Name = "СЕ (Черновицкая область)",
				Abbreviation = "СЕ",
				Published = true,
				DisplayOrder = 24
			});
			#endregion

			var countries = new List<Country>
								{
									ukr,
									new Country
									{
										Name = "Armenia",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "AM",
										ThreeLetterIsoCode = "ARM",
										NumericIsoCode = 51,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Azerbaijan",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "AZ",
										ThreeLetterIsoCode = "AZE",
										NumericIsoCode = 31,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Belarus",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "BY",
										ThreeLetterIsoCode = "BLR",
										NumericIsoCode = 112,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Georgia",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "GE",
										ThreeLetterIsoCode = "GEO",
										NumericIsoCode = 268,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Kazakhstan",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "KZ",
										ThreeLetterIsoCode = "KAZ",
										NumericIsoCode = 398,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Poland",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "PL",
										ThreeLetterIsoCode = "POL",
										NumericIsoCode = 616,
										SubjectToVat = true,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Russian Federation",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "RU",
										ThreeLetterIsoCode = "RUS",
										NumericIsoCode = 643,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Uzbekistan",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "UZ",
										ThreeLetterIsoCode = "UZB",
										NumericIsoCode = 860,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									 new Country
									{
										Name = "Tajikistan",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "TJ",
										ThreeLetterIsoCode = "TJK",
										NumericIsoCode = 762,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},
									new Country
									{
										Name = "Turkmenistan",
										AllowsBilling = true,
										AllowsShipping = true,
										TwoLetterIsoCode = "TM",
										ThreeLetterIsoCode = "TKM",
										NumericIsoCode = 795,
										SubjectToVat = false,
										DisplayOrder = 100,
										Published = true
									},

								};
			_countryRepository.Insert(countries);
		}

		protected virtual void InstallShippingMethods()
		{
			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

			var shippingSelf = new ShippingMethod
			{
				Name = "Самовывоз",
				Description = "Вы самостоятельно забираете товар из нашего офиса",
				DisplayOrder = 1
			};
			_shippingMethodRepository.Insert(shippingSelf);
			_localizedEntityService.SaveLocalizedValue(shippingSelf, c => c.Name, "Самовывоз", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(shippingSelf, c => c.Description, "Вы самостоятельно забираете товар из нашего офиса", languageRu.Id);

			_localizedEntityService.SaveLocalizedValue(shippingSelf, c => c.Name, "Самовивіз", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(shippingSelf, c => c.Description, "Ви самостійно забирєете товар з нашого офісу", languageUa.Id);

			var shippingNovaPosta = new ShippingMethod
			{
				Name = "Новая Почта",
				Description = "Мы отправляем Вам товар новой почтой",
				DisplayOrder = 2
			};
			_shippingMethodRepository.Insert(shippingNovaPosta);
			_localizedEntityService.SaveLocalizedValue(shippingNovaPosta, c => c.Name, "Новая Почта", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(shippingNovaPosta, c => c.Description, "Мы отправляем Вам товар новой почтой", languageRu.Id);
													   
			_localizedEntityService.SaveLocalizedValue(shippingNovaPosta, c => c.Name, "Нова Пошта", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(shippingNovaPosta, c => c.Description, "Ми відсилаємо Вам товар новою поштою", languageUa.Id);
		}

		protected virtual void InstallDeliveryDates()
		{
			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

			var days12 = new DeliveryDate
			{
				Name = "1-2 days",
				DisplayOrder = 1
			};
			_deliveryDateRepository.Insert(days12);
			_localizedEntityService.SaveLocalizedValue(days12, c => c.Name, "1-2 дня", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(days12, c => c.Name, "1-2 дні", languageUa.Id);

			var dasy35 = new DeliveryDate
			{
				Name = "3-5 days",
				DisplayOrder = 5
			};
			_deliveryDateRepository.Insert(dasy35);
			_localizedEntityService.SaveLocalizedValue(dasy35, c => c.Name, "3-5 дней", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(dasy35, c => c.Name, "3-5 днів", languageUa.Id);

			var dasy7 = new DeliveryDate
			{
				Name = "1 week",
				DisplayOrder = 10
			};
			_deliveryDateRepository.Insert(dasy7);
			_localizedEntityService.SaveLocalizedValue(dasy35, c => c.Name, "1 неделя", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(dasy35, c => c.Name, "1 тиждень", languageUa.Id);
		}

		protected virtual void InstallProductAvailabilityRanges()
		{
			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

			var rande24 = new ProductAvailabilityRange
			{
				Name = "2-4 days",
				DisplayOrder = 1
			};
			_productAvailabilityRangeRepository.Insert(rande24);
			_localizedEntityService.SaveLocalizedValue(rande24, c => c.Name, "2-4 дня", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(rande24, c => c.Name, "2-4 дні", languageUa.Id);

			var range710 = new ProductAvailabilityRange
			{
				Name = "7-10 days",
				DisplayOrder = 2
			};
			_productAvailabilityRangeRepository.Insert(range710);
			_localizedEntityService.SaveLocalizedValue(range710, c => c.Name, "7-10 дней", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(range710, c => c.Name, "7-10 днів", languageUa.Id);

			var range14 = new ProductAvailabilityRange
			{
				Name = "2 week",
				DisplayOrder = 3
			};
			_productAvailabilityRangeRepository.Insert(range14);
			_localizedEntityService.SaveLocalizedValue(range14, c => c.Name, "2 недели", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(range14, c => c.Name, "2 тиждны", languageUa.Id);

		}

		protected virtual void InstallCustomersAndUsers(string defaultUserEmail, string defaultUserPassword)
		{
			var crAdministrators = new CustomerRole
			{
				Name = "Administrators",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemCustomerRoleNames.Administrators,
			};
			var crForumModerators = new CustomerRole
			{
				Name = "Forum Moderators",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemCustomerRoleNames.ForumModerators,
			};
			var crRegistered = new CustomerRole
			{
				Name = "Registered",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemCustomerRoleNames.Registered,
			};
			var crGuests = new CustomerRole
			{
				Name = "Guests",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemCustomerRoleNames.Guests,
			};
			var crVendors = new CustomerRole
			{
				Name = "Vendors",
				Active = true,
				IsSystemRole = true,
				SystemName = SystemCustomerRoleNames.Vendors,
			};
			var customerRoles = new List<CustomerRole>
								{
									crAdministrators,
									crForumModerators,
									crRegistered,
									crGuests,
									crVendors
								};
			_customerRoleRepository.Insert(customerRoles);

			//default store 
			var defaultStore = _storeRepository.Table.FirstOrDefault();

			if (defaultStore == null)
				throw new Exception("No default store could be loaded");

			var storeId = defaultStore.Id;

			//admin user
			var adminUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = defaultUserEmail,
				Username = defaultUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};

			var defaultAdminUserAddress = new Address
			{
				FirstName = "John",
				LastName = "Smith",
				PhoneNumber = "12345678",
				Email = defaultUserEmail,
				FaxNumber = "",
				Company = "Nop Solutions Ltd",
				Address1 = "21 West 52nd Street",
				Address2 = "",
				City = "New York",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "New York"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				ZipPostalCode = "10021",
				CreatedOnUtc = DateTime.UtcNow,
			};
			adminUser.Addresses.Add(defaultAdminUserAddress);
			adminUser.BillingAddress = defaultAdminUserAddress;
			adminUser.ShippingAddress = defaultAdminUserAddress;

			adminUser.CustomerRoles.Add(crAdministrators);
			adminUser.CustomerRoles.Add(crForumModerators);
			adminUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(adminUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(adminUser, SystemCustomerAttributeNames.FirstName, "John");
			_genericAttributeService.SaveAttribute(adminUser, SystemCustomerAttributeNames.LastName, "Smith");

			//set hashed admin password
			var customerRegistrationService = EngineContext.Current.Resolve<ICustomerRegistrationService>();
			customerRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false,
				 PasswordFormat.Hashed, defaultUserPassword));

			//second user
			var secondUserEmail = "steve_gates@nopCommerce.com";
			var secondUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = secondUserEmail,
				Username = secondUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			var defaultSecondUserAddress = new Address
			{
				FirstName = "Steve",
				LastName = "Gates",
				PhoneNumber = "87654321",
				Email = secondUserEmail,
				FaxNumber = "",
				Company = "Steve Company",
				Address1 = "750 Bel Air Rd.",
				Address2 = "",
				City = "Los Angeles",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "California"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				ZipPostalCode = "90077",
				CreatedOnUtc = DateTime.UtcNow,
			};
			secondUser.Addresses.Add(defaultSecondUserAddress);
			secondUser.BillingAddress = defaultSecondUserAddress;
			secondUser.ShippingAddress = defaultSecondUserAddress;

			secondUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(secondUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(secondUser, SystemCustomerAttributeNames.FirstName, defaultSecondUserAddress.FirstName);
			_genericAttributeService.SaveAttribute(secondUser, SystemCustomerAttributeNames.LastName, defaultSecondUserAddress.LastName);

			//set customer password
			_customerPasswordRepository.Insert(new CustomerPassword
			{
				Customer = secondUser,
				Password = "123456",
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = string.Empty,
				CreatedOnUtc = DateTime.UtcNow
			});

			//third user
			var thirdUserEmail = "arthur_holmes@nopCommerce.com";
			var thirdUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = thirdUserEmail,
				Username = thirdUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			var defaultThirdUserAddress = new Address
			{
				FirstName = "Arthur",
				LastName = "Holmes",
				PhoneNumber = "111222333",
				Email = thirdUserEmail,
				FaxNumber = "",
				Company = "Holmes Company",
				Address1 = "221B Baker Street",
				Address2 = "",
				City = "London",
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "GBR"),
				ZipPostalCode = "NW1 6XE",
				CreatedOnUtc = DateTime.UtcNow,
			};
			thirdUser.Addresses.Add(defaultThirdUserAddress);
			thirdUser.BillingAddress = defaultThirdUserAddress;
			thirdUser.ShippingAddress = defaultThirdUserAddress;

			thirdUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(thirdUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(thirdUser, SystemCustomerAttributeNames.FirstName, defaultThirdUserAddress.FirstName);
			_genericAttributeService.SaveAttribute(thirdUser, SystemCustomerAttributeNames.LastName, defaultThirdUserAddress.LastName);

			//set customer password
			_customerPasswordRepository.Insert(new CustomerPassword
			{
				Customer = thirdUser,
				Password = "123456",
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = string.Empty,
				CreatedOnUtc = DateTime.UtcNow
			});

			//fourth user
			var fourthUserEmail = "james_pan@nopCommerce.com";
			var fourthUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = fourthUserEmail,
				Username = fourthUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			var defaultFourthUserAddress = new Address
			{
				FirstName = "James",
				LastName = "Pan",
				PhoneNumber = "369258147",
				Email = fourthUserEmail,
				FaxNumber = "",
				Company = "Pan Company",
				Address1 = "St Katharine’s West 16",
				Address2 = "",
				City = "St Andrews",
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "GBR"),
				ZipPostalCode = "KY16 9AX",
				CreatedOnUtc = DateTime.UtcNow,
			};
			fourthUser.Addresses.Add(defaultFourthUserAddress);
			fourthUser.BillingAddress = defaultFourthUserAddress;
			fourthUser.ShippingAddress = defaultFourthUserAddress;

			fourthUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(fourthUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(fourthUser, SystemCustomerAttributeNames.FirstName, defaultFourthUserAddress.FirstName);
			_genericAttributeService.SaveAttribute(fourthUser, SystemCustomerAttributeNames.LastName, defaultFourthUserAddress.LastName);

			//set customer password
			_customerPasswordRepository.Insert(new CustomerPassword
			{
				Customer = fourthUser,
				Password = "123456",
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = string.Empty,
				CreatedOnUtc = DateTime.UtcNow
			});

			//fifth user
			var fifthUserEmail = "brenda_lindgren@nopCommerce.com";
			var fifthUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = fifthUserEmail,
				Username = fifthUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			var defaultFifthUserAddress = new Address
			{
				FirstName = "Brenda",
				LastName = "Lindgren",
				PhoneNumber = "14785236",
				Email = fifthUserEmail,
				FaxNumber = "",
				Company = "Brenda Company",
				Address1 = "1249 Tongass Avenue, Suite B",
				Address2 = "",
				City = "Ketchikan",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "Alaska"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				ZipPostalCode = "99901",
				CreatedOnUtc = DateTime.UtcNow,
			};
			fifthUser.Addresses.Add(defaultFifthUserAddress);
			fifthUser.BillingAddress = defaultFifthUserAddress;
			fifthUser.ShippingAddress = defaultFifthUserAddress;

			fifthUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(fifthUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(fifthUser, SystemCustomerAttributeNames.FirstName, defaultFifthUserAddress.FirstName);
			_genericAttributeService.SaveAttribute(fifthUser, SystemCustomerAttributeNames.LastName, defaultFifthUserAddress.LastName);

			//set customer password
			_customerPasswordRepository.Insert(new CustomerPassword
			{
				Customer = fifthUser,
				Password = "123456",
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = string.Empty,
				CreatedOnUtc = DateTime.UtcNow
			});

			//sixth user
			var sixthUserEmail = "victoria_victoria@nopCommerce.com";
			var sixthUser = new Customer
			{
				CustomerGuid = Guid.NewGuid(),
				Email = sixthUserEmail,
				Username = sixthUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			var defaultSixthUserAddress = new Address
			{
				FirstName = "Victoria",
				LastName = "Terces",
				PhoneNumber = "45612378",
				Email = sixthUserEmail,
				FaxNumber = "",
				Company = "Terces Company",
				Address1 = "201 1st Avenue South",
				Address2 = "",
				City = "Saskatoon",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "Saskatchewan"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "CAN"),
				ZipPostalCode = "S7K 1J9",
				CreatedOnUtc = DateTime.UtcNow,
			};
			sixthUser.Addresses.Add(defaultSixthUserAddress);
			sixthUser.BillingAddress = defaultSixthUserAddress;
			sixthUser.ShippingAddress = defaultSixthUserAddress;

			sixthUser.CustomerRoles.Add(crRegistered);

			_customerRepository.Insert(sixthUser);
			//set default customer name
			_genericAttributeService.SaveAttribute(sixthUser, SystemCustomerAttributeNames.FirstName, defaultSixthUserAddress.FirstName);
			_genericAttributeService.SaveAttribute(sixthUser, SystemCustomerAttributeNames.LastName, defaultSixthUserAddress.LastName);

			//set customer password
			_customerPasswordRepository.Insert(new CustomerPassword
			{
				Customer = sixthUser,
				Password = "123456",
				PasswordFormat = PasswordFormat.Clear,
				PasswordSalt = string.Empty,
				CreatedOnUtc = DateTime.UtcNow
			});

			//search engine (crawler) built-in user
			var searchEngineUser = new Customer
			{
				Email = "builtin@search_engine_record.com",
				CustomerGuid = Guid.NewGuid(),
				AdminComment = "Built-in system guest record used for requests from search engines.",
				Active = true,
				IsSystemAccount = true,
				SystemName = SystemCustomerNames.SearchEngine,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			searchEngineUser.CustomerRoles.Add(crGuests);
			_customerRepository.Insert(searchEngineUser);


			//built-in user for background tasks
			var backgroundTaskUser = new Customer
			{
				Email = "builtin@background-task-record.com",
				CustomerGuid = Guid.NewGuid(),
				AdminComment = "Built-in system record used for background tasks.",
				Active = true,
				IsSystemAccount = true,
				SystemName = SystemCustomerNames.BackgroundTask,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInStoreId = storeId
			};
			backgroundTaskUser.CustomerRoles.Add(crGuests);
			_customerRepository.Insert(backgroundTaskUser);
		}

		protected virtual void InstallOrders()
		{

		}

		protected virtual void InstallActivityLog(string defaultUserEmail)
		{
		}

		protected virtual void InstallSearchTerms()
		{
		}

		protected virtual void InstallEmailAccounts()
		{
			var emailAccounts = new List<EmailAccount>
							   {
								   new EmailAccount
									   {
										   Email = "test@mail.com",
										   DisplayName = "Store name",
										   Host = "smtp.mail.com",
										   Port = 25,
										   Username = "123",
										   Password = "123",
										   EnableSsl = false,
										   UseDefaultCredentials = false
									   },
							   };
			_emailAccountRepository.Insert(emailAccounts);
		}

		protected virtual void InstallMessageTemplates()
		{
			var eaGeneral = _emailAccountRepository.Table.FirstOrDefault();
			if (eaGeneral == null)
				throw new Exception("Default email account cannot be loaded");

			var messageTemplates = new List<MessageTemplate>
			{
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.BlogCommentNotification,
					Subject = "%Store.Name%. New blog comment.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new blog comment has been created for blog post \"%BlogComment.BlogPostTitle%\".{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.BackInStockNotification,
					Subject = "%Store.Name%. Back in stock notification",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%,{0}<br />{0}Product <a target=\"_blank\" href=\"%BackInStockSubscription.ProductUrl%\">%BackInStockSubscription.ProductName%</a> is in stock.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.CustomerEmailValidationMessage,
					Subject = "%Store.Name%. Email validation",
					Body = string.Format("<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}To activate your account <a href=\"%Customer.AccountActivationURL%\">click here</a>.{0}<br />{0}<br />{0}%Store.Name%{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.CustomerEmailRevalidationMessage,
					Subject = "%Store.Name%. Email validation",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%!{0}<br />{0}To validate your new email address <a href=\"%Customer.EmailRevalidationURL%\">click here</a>.{0}<br />{0}<br />{0}%Store.Name%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.PrivateMessageNotification,
					Subject = "%Store.Name%. You have received a new private message",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}You have received a new private message.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.CustomerPasswordRecoveryMessage,
					Subject = "%Store.Name%. Password recovery",
					Body = string.Format("<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}To change your password <a href=\"%Customer.PasswordRecoveryURL%\">click here</a>.{0}<br />{0}<br />{0}%Store.Name%{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.CustomerWelcomeMessage,
					Subject = "Welcome to %Store.Name%",
					Body = string.Format("We welcome you to <a href=\"%Store.URL%\"> %Store.Name%</a>.{0}<br />{0}<br />{0}You can now take part in the various services we have to offer you. Some of these services include:{0}<br />{0}<br />{0}Permanent Cart - Any products added to your online cart remain there until you remove them, or check them out.{0}<br />{0}Address Book - We can now deliver your products to another address other than yours! This is perfect to send birthday gifts direct to the birthday-person themselves.{0}<br />{0}Order History - View your history of purchases that you have made with us.{0}<br />{0}Products Reviews - Share your opinions on products with our other customers.{0}<br />{0}<br />{0}For help with any of our online services, please email the store-owner: <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{0}<br />{0}<br />{0}Note: This email address was provided on our registration page. If you own the email and did not register on our site, please send an email to <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewForumPostMessage,
					Subject = "%Store.Name%. New Post Notification.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new post has been created in the topic <a href=\"%Forums.TopicURL%\">\"%Forums.TopicName%\"</a> at <a href=\"%Forums.ForumURL%\">\"%Forums.ForumName%\"</a> forum.{0}<br />{0}<br />{0}Click <a href=\"%Forums.TopicURL%\">here</a> for more info.{0}<br />{0}<br />{0}Post author: %Forums.PostAuthor%{0}<br />{0}Post body: %Forums.PostBody%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewForumTopicMessage,
					Subject = "%Store.Name%. New Topic Notification.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new topic <a href=\"%Forums.TopicURL%\">\"%Forums.TopicName%\"</a> has been created at <a href=\"%Forums.ForumURL%\">\"%Forums.ForumName%\"</a> forum.{0}<br />{0}<br />{0}Click <a href=\"%Forums.TopicURL%\">here</a> for more info.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.GiftCardNotification,
					Subject = "%GiftCard.SenderName% has sent you a gift card for %Store.Name%",
					Body = string.Format("<p>{0}You have received a gift card for %Store.Name%{0}</p>{0}<p>{0}Dear %GiftCard.RecipientName%,{0}<br />{0}<br />{0}%GiftCard.SenderName% (%GiftCard.SenderEmail%) has sent you a %GiftCard.Amount% gift cart for <a href=\"%Store.URL%\"> %Store.Name%</a>{0}</p>{0}<p>{0}You gift card code is %GiftCard.CouponCode%{0}</p>{0}<p>{0}%GiftCard.Message%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.CustomerRegisteredNotification,
					Subject = "%Store.Name%. New customer registration",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new customer registered with your store. Below are the customer's details:{0}<br />{0}Full name: %Customer.FullName%{0}<br />{0}Email: %Customer.Email%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewReturnRequestStoreOwnerNotification,
					Subject = "%Store.Name%. New return request.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Customer.FullName% has just submitted a new return request. Details are below:{0}<br />{0}Request ID: %ReturnRequest.CustomNumber%{0}<br />{0}Product: %ReturnRequest.Product.Quantity% x Product: %ReturnRequest.Product.Name%{0}<br />{0}Reason for return: %ReturnRequest.Reason%{0}<br />{0}Requested action: %ReturnRequest.RequestedAction%{0}<br />{0}Customer comments:{0}<br />{0}%ReturnRequest.CustomerComment%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewReturnRequestCustomerNotification,
					Subject = "%Store.Name%. New return request.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%!{0}<br />{0}You have just submitted a new return request. Details are below:{0}<br />{0}Request ID: %ReturnRequest.CustomNumber%{0}<br />{0}Product: %ReturnRequest.Product.Quantity% x Product: %ReturnRequest.Product.Name%{0}<br />{0}Reason for return: %ReturnRequest.Reason%{0}<br />{0}Requested action: %ReturnRequest.RequestedAction%{0}<br />{0}Customer comments:{0}<br />{0}%ReturnRequest.CustomerComment%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewsCommentNotification,
					Subject = "%Store.Name%. New news comment.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new news comment has been created for news \"%NewsComment.NewsTitle%\".{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewsletterSubscriptionActivationMessage,
					Subject = "%Store.Name%. Subscription activation message.",
					Body = string.Format("<p>{0}<a href=\"%NewsLetterSubscription.ActivationUrl%\">Click here to confirm your subscription to our list.</a>{0}</p>{0}<p>{0}If you received this email by mistake, simply delete it.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewsletterSubscriptionDeactivationMessage,
					Subject = "%Store.Name%. Subscription deactivation message.",
					Body = string.Format("<p>{0}<a href=\"%NewsLetterSubscription.DeactivationUrl%\">Click here to unsubscribe from our newsletter.</a>{0}</p>{0}<p>{0}If you received this email by mistake, simply delete it.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewVatSubmittedStoreOwnerNotification,
					Subject = "%Store.Name%. New VAT number is submitted.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Customer.FullName% (%Customer.Email%) has just submitted a new VAT number. Details are below:{0}<br />{0}VAT number: %Customer.VatNumber%{0}<br />{0}VAT number status: %Customer.VatNumberStatus%{0}<br />{0}Received name: %VatValidationResult.Name%{0}<br />{0}Received address: %VatValidationResult.Address%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderCancelledCustomerNotification,
					Subject = "%Store.Name%. Your order cancelled",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Your order has been cancelled. Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a target=\"_blank\" href=\"%Order.OrderURLForCustomer%\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderCompletedCustomerNotification,
					Subject = "%Store.Name%. Your order completed",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Your order has been completed. Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a target=\"_blank\" href=\"%Order.OrderURLForCustomer%\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ShipmentDeliveredCustomerNotification,
					Subject = "Your order from %Store.Name% has been delivered.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\"> %Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Good news! You order has been delivered.{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% Delivered Products:{0}<br />{0}<br />{0}%Shipment.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPlacedCustomerNotification,
					Subject = "Order receipt from %Store.Name%.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Thanks for buying from <a href=\"%Store.URL%\">%Store.Name%</a>. Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a target=\"_blank\" href=\"%Order.OrderURLForCustomer%\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPlacedStoreOwnerNotification,
					Subject = "%Store.Name%. Purchase Receipt for Order #%Order.OrderNumber%",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Order.CustomerFullName% (%Order.CustomerEmail%) has just placed an order from your store. Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ShipmentSentCustomerNotification,
					Subject = "Your order from %Store.Name% has been shipped.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\"> %Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%!,{0}<br />{0}Good news! You order has been shipped.{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% Shipped Products:{0}<br />{0}<br />{0}%Shipment.Product(s)%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ProductReviewNotification,
					Subject = "%Store.Name%. New product review.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}A new product review has been written for product \"%ProductReview.ProductName%\".{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.QuantityBelowStoreOwnerNotification,
					Subject = "%Store.Name%. Quantity below notification. %Product.Name%",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Product.Name% (ID: %Product.ID%) low quantity.{0}<br />{0}<br />{0}Quantity: %Product.StockQuantity%{0}<br />{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.QuantityBelowAttributeCombinationStoreOwnerNotification,
					Subject = "%Store.Name%. Quantity below notification. %Product.Name%",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Product.Name% (ID: %Product.ID%) low quantity.{0}<br />{0}%AttributeCombination.Formatted%{0}<br />{0}Quantity: %AttributeCombination.StockQuantity%{0}<br />{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ReturnRequestStatusChangedCustomerNotification,
					Subject = "%Store.Name%. Return request status was changed.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%,{0}<br />{0}Your return request #%ReturnRequest.CustomNumber% status has been changed.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.EmailAFriendMessage,
					Subject = "%Store.Name%. Referred Item",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\"> %Store.Name%</a>{0}<br />{0}<br />{0}%EmailAFriend.Email% was shopping on %Store.Name% and wanted to share the following item with you.{0}<br />{0}<br />{0}<b><a target=\"_blank\" href=\"%Product.ProductURLForCustomer%\">%Product.Name%</a></b>{0}<br />{0}%Product.ShortDescription%{0}<br />{0}<br />{0}For more info click <a target=\"_blank\" href=\"%Product.ProductURLForCustomer%\">here</a>{0}<br />{0}<br />{0}<br />{0}%EmailAFriend.PersonalMessage%{0}<br />{0}<br />{0}%Store.Name%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.WishlistToFriendMessage,
					Subject = "%Store.Name%. Wishlist",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\"> %Store.Name%</a>{0}<br />{0}<br />{0}%Wishlist.Email% was shopping on %Store.Name% and wanted to share a wishlist with you.{0}<br />{0}<br />{0}<br />{0}For more info click <a target=\"_blank\" href=\"%Wishlist.URLForCustomer%\">here</a>{0}<br />{0}<br />{0}<br />{0}%Wishlist.PersonalMessage%{0}<br />{0}<br />{0}%Store.Name%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewOrderNoteAddedCustomerNotification,
					Subject = "%Store.Name%. New order note has been added",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%,{0}<br />{0}New order note has been added to your account:{0}<br />{0}\"%Order.NewNoteText%\".{0}<br />{0}<a target=\"_blank\" href=\"%Order.OrderURLForCustomer%\">%Order.OrderURLForCustomer%</a>{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.RecurringPaymentCancelledStoreOwnerNotification,
					Subject = "%Store.Name%. Recurring payment cancelled",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%if (%RecurringPayment.CancelAfterFailedPayment%) The last payment for the recurring payment ID=%RecurringPayment.ID% failed, so it was cancelled. endif% %if (!%RecurringPayment.CancelAfterFailedPayment%) %Customer.FullName% (%Customer.Email%) has just cancelled a recurring payment ID=%RecurringPayment.ID%. endif%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.RecurringPaymentCancelledCustomerNotification,
					Subject = "%Store.Name%. Recurring payment cancelled",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%,{0}<br />{0}%if (%RecurringPayment.CancelAfterFailedPayment%) It appears your credit card didn't go through for this recurring payment (<a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>){0}<br />{0}So your subscription has been canceled. endif% %if (!%RecurringPayment.CancelAfterFailedPayment%) The recurring payment ID=%RecurringPayment.ID% was cancelled. endif%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.RecurringPaymentFailedCustomerNotification,
					Subject = "%Store.Name%. Last recurring payment failed",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Customer.FullName%,{0}<br />{0}It appears your credit card didn't go through for this recurring payment (<a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>){0}<br /> %if (%RecurringPayment.RecurringPaymentType% == \"Manual\") {0}You can recharge balance and manually retry payment or cancel it on the order history page. endif% %if (%RecurringPayment.RecurringPaymentType% == \"Automatic\") {0}You can recharge balance and wait, we will try to make the payment again, or you can cancel it on the order history page. endif%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPlacedVendorNotification,
					Subject = "%Store.Name%. Order placed",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Customer.FullName% (%Customer.Email%) has just placed an order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}%Order.Product(s)%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderRefundedCustomerNotification,
					Subject = "%Store.Name%. Order #%Order.OrderNumber% refunded",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Thanks for buying from <a href=\"%Store.URL%\">%Store.Name%</a>. Order #%Order.OrderNumber% has been has been refunded. Please allow 7-14 days for the refund to be reflected in your account.{0}<br />{0}<br />{0}Amount refunded: %Order.AmountRefunded%{0}<br />{0}<br />{0}Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br /{0}>Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderRefundedStoreOwnerNotification,
					Subject = "%Store.Name%. Order #%Order.OrderNumber% refunded",
					Body = string.Format("%Store.Name%. Order #%Order.OrderNumber% refunded', N'{0}<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Order #%Order.OrderNumber% has been just refunded{0}<br />{0}<br />{0}Amount refunded: %Order.AmountRefunded%{0}<br />{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPaidStoreOwnerNotification,
					Subject = "%Store.Name%. Order #%Order.OrderNumber% paid",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Order #%Order.OrderNumber% has been just paid{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPaidCustomerNotification,
					Subject = "%Store.Name%. Order #%Order.OrderNumber% paid",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Hello %Order.CustomerFullName%,{0}<br />{0}Thanks for buying from <a href=\"%Store.URL%\">%Store.Name%</a>. Order #%Order.OrderNumber% has been just paid. Below is the summary of the order.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Order Details: <a href=\"%Order.OrderURLForCustomer%\" target=\"_blank\">%Order.OrderURLForCustomer%</a>{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}<br />{0}<br />{0}Billing Address{0}<br />{0}%Order.BillingFirstName% %Order.BillingLastName%{0}<br />{0}%Order.BillingAddress1%{0}<br />{0}%Order.BillingCity% %Order.BillingZipPostalCode%{0}<br />{0}%Order.BillingStateProvince% %Order.BillingCountry%{0}<br />{0}<br />{0}<br />{0}<br />{0}%if (%Order.Shippable%) Shipping Address{0}<br />{0}%Order.ShippingFirstName% %Order.ShippingLastName%{0}<br />{0}%Order.ShippingAddress1%{0}<br />{0}%Order.ShippingCity% %Order.ShippingZipPostalCode%{0}<br />{0}%Order.ShippingStateProvince% %Order.ShippingCountry%{0}<br />{0}<br />{0}Shipping Method: %Order.ShippingMethod%{0}<br />{0}<br />{0} endif% %Order.Product(s)%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.OrderPaidVendorNotification,
					Subject = "%Store.Name%. Order #%Order.OrderNumber% paid",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Order #%Order.OrderNumber% has been just paid.{0}<br />{0}<br />{0}Order Number: %Order.OrderNumber%{0}<br />{0}Date Ordered: %Order.CreatedOn%{0}<br />{0}<br />{0}%Order.Product(s)%{0}</p>{0}", Environment.NewLine),
                    //this template is disabled by default
                    IsActive = false,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.NewVendorAccountApplyStoreOwnerNotification,
					Subject = "%Store.Name%. New vendor account submitted.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}%Customer.FullName% (%Customer.Email%) has just submitted for a vendor account. Details are below:{0}<br />{0}Vendor name: %Vendor.Name%{0}<br />{0}Vendor email: %Vendor.Email%{0}<br />{0}<br />{0}You can activate it in admin area.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.VendorInformationChangeNotification,
					Subject = "%Store.Name%. Vendor information change.",
					Body = string.Format("<p>{0}<a href=\"%Store.URL%\">%Store.Name%</a>{0}<br />{0}<br />{0}Vendor %Vendor.Name% (%Vendor.Email%) has just changed information about itself.{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ContactUsMessage,
					Subject = "%Store.Name%. Contact us",
					Body = string.Format("<p>{0}%ContactUs.Body%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				},
				new MessageTemplate
				{
					Name = MessageTemplateSystemNames.ContactVendorMessage,
					Subject = "%Store.Name%. Contact us",
					Body = string.Format("<p>{0}%ContactUs.Body%{0}</p>{0}", Environment.NewLine),
					IsActive = true,
					EmailAccountId = eaGeneral.Id,
				}
			};
			_messageTemplateRepository.Insert(messageTemplates);
		}

		protected virtual void InstallTopics()
		{
			var defaultTopicTemplate =
				_topicTemplateRepository.Table.FirstOrDefault(tt => tt.Name == "Default template");
			if (defaultTopicTemplate == null)
				throw new Exception("Topic template cannot be loaded");

			var topics = new List<Topic>
							   {
								   new Topic
									   {
										   SystemName = "AboutUs",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   IncludeInFooterColumn1 = true,
										   DisplayOrder = 20,
										   Published = true,
										   Title = "About us",
										   Body = "<p>Put your &quot;About Us&quot; information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "CheckoutAsGuestOrRegister",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "",
										   Body = "<p><strong>Register and save time!</strong><br />Register with us for future convenience:</p><ul><li>Fast and easy check out</li><li>Easy access to your order history and status</li></ul>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ConditionsOfUse",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   IncludeInFooterColumn1 = true,
										   DisplayOrder = 15,
										   Published = true,
										   Title = "Conditions of Use",
										   Body = "<p>Put your conditions of use information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ContactUs",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "",
										   Body = "<p>Put your contact information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ForumWelcomeMessage",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "Forums",
										   Body = "<p>Put your welcome message here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "HomePageText",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "Welcome to our store",
										   Body = "<p>Online shopping is the process consumers go through to purchase products or services over the Internet. You can edit this in the admin site.</p><p>If you have questions, see the <a href=\"http://www.nopcommerce.com/documentation.aspx\">Documentation</a>, or post in the <a href=\"http://www.nopcommerce.com/boards/\">Forums</a> at <a href=\"http://www.nopcommerce.com\">nopCommerce.com</a></p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "LoginRegistrationInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "About login / registration",
										   Body = "<p>Put your login / registration information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "PrivacyInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   IncludeInFooterColumn1 = true,
										   DisplayOrder = 10,
										   Published = true,
										   Title = "Privacy notice",
										   Body = "<p>Put your privacy policy information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "PageNotFound",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "",
										   Body = "<p><strong>The page you requested was not found, and we have a fine guess why.</strong></p><ul><li>If you typed the URL directly, please make sure the spelling is correct.</li><li>The page no longer exists. In this case, we profusely apologize for the inconvenience and for any damage this may cause.</li></ul>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ShippingInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   IncludeInFooterColumn1 = true,
										   DisplayOrder = 5,
										   Published = true,
										   Title = "Shipping & returns",
										   Body = "<p>Put your shipping &amp; returns information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ApplyVendor",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = true,
										   Title = "",
										   Body = "<p>Put your apply vendor instructions here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
							   };
			_topicRepository.Insert(topics);


			//search engine names
			foreach (var topic in topics)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = topic.Id,
					EntityName = "Topic",
					LanguageId = 0,
					IsActive = true,
					Slug = topic.ValidateSeName("", !String.IsNullOrEmpty(topic.Title) ? topic.Title : topic.SystemName, true)
				});
			}

		}

		protected virtual void InstallSettings()
		{
			var settingService = EngineContext.Current.Resolve<ISettingService>();
			settingService.SaveSetting(new PdfSettings
			{
				LogoPictureId = 0,
				LetterPageSizeEnabled = false,
				RenderOrderNotes = true,
				FontFileName = "FreeSerif.ttf",
				InvoiceFooterTextColumn1 = null,
				InvoiceFooterTextColumn2 = null,
			});

			settingService.SaveSetting(new CommonSettings
			{
				UseSystemEmailForContactUsForm = true,
				UseStoredProceduresIfSupported = true,
				SitemapEnabled = true,
				SitemapIncludeCategories = true,
				SitemapIncludeManufacturers = true,
				SitemapIncludeProducts = false,
				DisplayJavaScriptDisabledWarning = false,
				UseFullTextSearch = false,
				FullTextMode = FulltextSearchMode.ExactMatch,
				Log404Errors = true,
				BreadcrumbDelimiter = "/",
				RenderXuaCompatible = false,
				XuaCompatibleValue = "IE=edge",
				BbcodeEditorOpenLinksInNewWindow = false
			});

			settingService.SaveSetting(new SeoSettings
			{
				PageTitleSeparator = ". ",
				PageTitleSeoAdjustment = PageTitleSeoAdjustment.PagenameAfterStorename,
				DefaultTitle = "MBN Pro",
				DefaultMetaKeywords = "",
				DefaultMetaDescription = "",
				GenerateProductMetaDescription = true,
				ConvertNonWesternChars = false,
				AllowUnicodeCharsInUrls = true,
				CanonicalUrlsEnabled = false,
				WwwRequirement = WwwRequirement.NoMatter,
				//we disable bundling out of the box because it requires a lot of server resources
				EnableJsBundling = false,
				EnableCssBundling = false,
				TwitterMetaTags = true,
				OpenGraphMetaTags = true,
				ReservedUrlRecordSlugs = new List<string>
				{
					"admin",
					"install",
					"recentlyviewedproducts",
					"newproducts",
					"compareproducts",
					"clearcomparelist",
					"setproductreviewhelpfulness",
					"login",
					"register",
					"logout",
					"cart",
					"wishlist",
					"emailwishlist",
					"checkout",
					"onepagecheckout",
					"contactus",
					"passwordrecovery",
					"subscribenewsletter",
					"blog",
					"boards",
					"inboxupdate",
					"sentupdate",
					"news",
					"sitemap",
					"search",
					"config",
					"eucookielawaccept",
					"page-not-found",
                    //system names are not allowed (anyway they will cause a runtime error),
                    "con",
					"lpt1",
					"lpt2",
					"lpt3",
					"lpt4",
					"lpt5",
					"lpt6",
					"lpt7",
					"lpt8",
					"lpt9",
					"com1",
					"com2",
					"com3",
					"com4",
					"com5",
					"com6",
					"com7",
					"com8",
					"com9",
					"null",
					"prn",
					"aux"
				},
				CustomHeadTags = ""
			});

			settingService.SaveSetting(new AdminAreaSettings
			{
				DefaultGridPageSize = 15,
				PopupGridPageSize = 10,
				GridPageSizes = "10, 15, 20, 50, 100",
				RichEditorAdditionalSettings = null,
				RichEditorAllowJavaScript = false
			});


			settingService.SaveSetting(new ProductEditorSettings
			{
				Weight = true,
				Dimensions = true,
				ProductAttributes = true,
				SpecificationAttributes = true
			});

			settingService.SaveSetting(new CatalogSettings
			{
				AllowViewUnpublishedProductPage = true,
				DisplayDiscontinuedMessageForUnpublishedProducts = true,
				PublishBackProductWhenCancellingOrders = false,
				ShowSkuOnProductDetailsPage = true,
				ShowSkuOnCatalogPages = false,
				ShowManufacturerPartNumber = false,
				ShowGtin = false,
				ShowFreeShippingNotification = true,
				AllowProductSorting = true,
				AllowProductViewModeChanging = true,
				DefaultViewMode = "grid",
				ShowProductsFromSubcategories = false,
				ShowCategoryProductNumber = false,
				ShowCategoryProductNumberIncludingSubcategories = false,
				CategoryBreadcrumbEnabled = true,
				ShowShareButton = true,
				PageShareCode = "<!-- AddThis Button BEGIN --><div class=\"addthis_toolbox addthis_default_style \"><a class=\"addthis_button_preferred_1\"></a><a class=\"addthis_button_preferred_2\"></a><a class=\"addthis_button_preferred_3\"></a><a class=\"addthis_button_preferred_4\"></a><a class=\"addthis_button_compact\"></a><a class=\"addthis_counter addthis_bubble_style\"></a></div><script type=\"text/javascript\" src=\"http://s7.addthis.com/js/250/addthis_widget.js#pubid=nopsolutions\"></script><!-- AddThis Button END -->",
				ProductReviewsMustBeApproved = false,
				DefaultProductRatingValue = 5,
				AllowAnonymousUsersToReviewProduct = false,
				ProductReviewPossibleOnlyAfterPurchasing = false,
				NotifyStoreOwnerAboutNewProductReviews = false,
				EmailAFriendEnabled = true,
				AllowAnonymousUsersToEmailAFriend = false,
				RecentlyViewedProductsNumber = 3,
				RecentlyViewedProductsEnabled = true,
				NewProductsNumber = 6,
				NewProductsEnabled = true,
				CompareProductsEnabled = true,
				CompareProductsNumber = 4,
				ProductSearchAutoCompleteEnabled = true,
				ProductSearchAutoCompleteNumberOfProducts = 10,
				ProductSearchTermMinimumLength = 3,
				ShowProductImagesInSearchAutoComplete = false,
				ShowBestsellersOnHomepage = false,
				NumberOfBestsellersOnHomepage = 4,
				SearchPageProductsPerPage = 6,
				SearchPageAllowCustomersToSelectPageSize = true,
				SearchPagePageSizeOptions = "6, 3, 9, 18",
				ProductsAlsoPurchasedEnabled = true,
				ProductsAlsoPurchasedNumber = 4,
				AjaxProcessAttributeChange = true,
				NumberOfProductTags = 15,
				ProductsByTagPageSize = 6,
				IncludeShortDescriptionInCompareProducts = false,
				IncludeFullDescriptionInCompareProducts = false,
				IncludeFeaturedProductsInNormalLists = false,
				DisplayTierPricesWithDiscounts = true,
				IgnoreDiscounts = false,
				IgnoreFeaturedProducts = false,
				IgnoreAcl = true,
				IgnoreStoreLimitations = true,
				CacheProductPrices = false,
				ProductsByTagAllowCustomersToSelectPageSize = true,
				ProductsByTagPageSizeOptions = "6, 3, 9, 18",
				MaximumBackInStockSubscriptions = 200,
				ManufacturersBlockItemsToDisplay = 2,
				DisplayTaxShippingInfoFooter = false,
				DisplayTaxShippingInfoProductDetailsPage = false,
				DisplayTaxShippingInfoProductBoxes = false,
				DisplayTaxShippingInfoShoppingCart = false,
				DisplayTaxShippingInfoWishlist = false,
				DisplayTaxShippingInfoOrderDetailsPage = false,
				DefaultCategoryPageSizeOptions = "6, 3, 9",
				DefaultCategoryPageSize = 6,
				DefaultManufacturerPageSizeOptions = "6, 3, 9",
				DefaultManufacturerPageSize = 6,
				ShowProductReviewsTabOnAccountPage = true,
				ProductReviewsPageSizeOnAccountPage = 10,
				ExportImportProductAttributes = true,
				ExportImportUseDropdownlistsForAssociatedEntities = true
			});

			settingService.SaveSetting(new LocalizationSettings
			{
				DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.Name == "Russian").Id,
				UseImagesForLanguageSelection = false,
				SeoFriendlyUrlsForLanguagesEnabled = false,
				AutomaticallyDetectLanguage = false,
				LoadAllLocaleRecordsOnStartup = true,
				LoadAllLocalizedPropertiesOnStartup = true,
				LoadAllUrlRecordsOnStartup = false,
				IgnoreRtlPropertyForAdminArea = false
			});

			settingService.SaveSetting(new CustomerSettings
			{
				UsernamesEnabled = false,
				CheckUsernameAvailabilityEnabled = false,
				AllowUsersToChangeUsernames = false,
				DefaultPasswordFormat = PasswordFormat.Hashed,
				HashedPasswordFormat = "SHA1",
				PasswordMinLength = 6,
				UnduplicatedPasswordsNumber = 4,
				PasswordRecoveryLinkDaysValid = 7,
				PasswordLifetime = 90,
				FailedPasswordAllowedAttempts = 0,
				FailedPasswordLockoutMinutes = 30,
				UserRegistrationType = UserRegistrationType.Standard,
				AllowCustomersToUploadAvatars = false,
				AvatarMaximumSizeBytes = 20000,
				DefaultAvatarEnabled = true,
				ShowCustomersLocation = false,
				ShowCustomersJoinDate = false,
				AllowViewingProfiles = false,
				NotifyNewCustomerRegistration = false,
				HideDownloadableProductsTab = false,
				HideBackInStockSubscriptionsTab = false,
				DownloadableProductsValidateUser = false,
				CustomerNameFormat = CustomerNameFormat.ShowFirstName,
				GenderEnabled = true,
				DateOfBirthEnabled = true,
				DateOfBirthRequired = false,
				DateOfBirthMinimumAge = null,
				CompanyEnabled = true,
				StreetAddressEnabled = false,
				StreetAddress2Enabled = false,
				ZipPostalCodeEnabled = false,
				CityEnabled = false,
				CountryEnabled = false,
				CountryRequired = false,
				StateProvinceEnabled = false,
				StateProvinceRequired = false,
				PhoneEnabled = false,
				FaxEnabled = false,
				AcceptPrivacyPolicyEnabled = false,
				NewsletterEnabled = true,
				NewsletterTickedByDefault = true,
				HideNewsletterBlock = false,
				NewsletterBlockAllowToUnsubscribe = false,
				OnlineCustomerMinutes = 20,
				StoreLastVisitedPage = false,
				SuffixDeletedCustomers = false,
				EnteringEmailTwice = false,
				RequireRegistrationForDownloadableProducts = false,
				DeleteGuestTaskOlderThanMinutes = 1440
			});

			settingService.SaveSetting(new AddressSettings
			{
				CompanyEnabled = true,
				StreetAddressEnabled = true,
				StreetAddressRequired = true,
				StreetAddress2Enabled = true,
				ZipPostalCodeEnabled = true,
				ZipPostalCodeRequired = true,
				CityEnabled = true,
				CityRequired = true,
				CountryEnabled = true,
				StateProvinceEnabled = true,
				PhoneEnabled = true,
				PhoneRequired = true,
				FaxEnabled = true,
			});

			settingService.SaveSetting(new MediaSettings
			{
				AvatarPictureSize = 120,
				ProductThumbPictureSize = 415,
				ProductDetailsPictureSize = 550,
				ProductThumbPictureSizeOnProductDetailsPage = 100,
				AssociatedProductPictureSize = 220,
				CategoryThumbPictureSize = 450,
				ManufacturerThumbPictureSize = 420,
				VendorThumbPictureSize = 450,
				CartThumbPictureSize = 80,
				MiniCartThumbPictureSize = 70,
				AutoCompleteSearchThumbPictureSize = 20,
				ImageSquarePictureSize = 32,
				MaximumImageSize = 1980,
				DefaultPictureZoomEnabled = false,
				DefaultImageQuality = 80,
				MultipleThumbDirectories = false,
				ImportProductImagesUsingHash = true
			});
			//pictures
			var pictureService = EngineContext.Current.Resolve<IPictureService>();
			var sampleImagesPath = CommonHelper.MapPath("~/Themes/NopElectro/content/images/");
			settingService.SaveSetting(new StoreInformationSettings
			{
				StoreClosed = false,
				DefaultStoreTheme = "NopElectro",
				LogoPictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "MBNPRO.jpg"), MimeTypes.ImageJpeg,
					pictureService.GetPictureSeName("MBN Pro")).Id,
				AllowCustomerToSelectTheme = false,
				DisplayMiniProfilerInPublicStore = false,
				DisplayEuCookieLawWarning = false,
				FacebookLink = "http://www.facebook.com/nopCommerce",
				TwitterLink = "https://twitter.com/nopCommerce",
				YoutubeLink = "http://www.youtube.com/user/nopCommerce",
				GooglePlusLink = "https://plus.google.com/+nopcommerce",
				HidePoweredByNopCommerce = false
			});

			settingService.SaveSetting(new ExternalAuthenticationSettings
			{
				AutoRegisterEnabled = true,
				RequireEmailValidation = false
			});

			settingService.SaveSetting(new RewardPointsSettings
			{
				Enabled = true,
				ExchangeRate = 1,
				PointsForRegistration = 0,
				PointsForPurchases_Amount = 10,
				PointsForPurchases_Points = 1,
				ActivationDelay = 0,
				ActivationDelayPeriodId = 0,
				DisplayHowMuchWillBeEarned = true,
				PointsAccumulatedForAllStores = true,
				PageSize = 10
			});

			settingService.SaveSetting(new CurrencySettings
			{
				DisplayCurrencyLabel = false,
				PrimaryStoreCurrencyId = _currencyRepository.Table.Single(c => c.CurrencyCode == "USD").Id,
				PrimaryExchangeRateCurrencyId = _currencyRepository.Table.Single(c => c.CurrencyCode == "USD").Id,
				ActiveExchangeRateProviderSystemName = "CurrencyExchange.MoneyConverter",
				AutoUpdateEnabled = false
			});

			settingService.SaveSetting(new MeasureSettings
			{
				BaseDimensionId = _measureDimensionRepository.Table.Single(m => m.SystemKeyword == "meters").Id,
				BaseWeightId = _measureWeightRepository.Table.Single(m => m.SystemKeyword == "kg").Id,
			});

			settingService.SaveSetting(new MessageTemplatesSettings
			{
				CaseInvariantReplacement = false,
				Color1 = "#b9babe",
				Color2 = "#ebecee",
				Color3 = "#dde2e6",
			});

			settingService.SaveSetting(new ShoppingCartSettings
			{
				DisplayCartAfterAddingProduct = false,
				DisplayWishlistAfterAddingProduct = false,
				MaximumShoppingCartItems = 1000,
				MaximumWishlistItems = 1000,
				AllowOutOfStockItemsToBeAddedToWishlist = false,
				MoveItemsFromWishlistToCart = true,
				CartsSharedBetweenStores = false,
				ShowProductImagesOnShoppingCart = true,
				ShowProductImagesOnWishList = true,
				ShowDiscountBox = true,
				ShowGiftCardBox = true,
				CrossSellsNumber = 4,
				EmailWishlistEnabled = true,
				AllowAnonymousUsersToEmailWishlist = false,
				MiniShoppingCartEnabled = true,
				ShowProductImagesInMiniShoppingCart = true,
				MiniShoppingCartProductNumber = 5,
				RoundPricesDuringCalculation = true,
				GroupTierPricesForDistinctShoppingCartItems = false,
				AllowCartItemEditing = true,
				RenderAssociatedAttributeValueQuantity = true
			});

			settingService.SaveSetting(new OrderSettings
			{
				ReturnRequestNumberMask = "{ID}",
				IsReOrderAllowed = true,
				MinOrderSubtotalAmount = 0,
				MinOrderSubtotalAmountIncludingTax = false,
				MinOrderTotalAmount = 0,
				AutoUpdateOrderTotalsOnEditingOrder = false,
				AnonymousCheckoutAllowed = true,
				TermsOfServiceOnShoppingCartPage = true,
				TermsOfServiceOnOrderConfirmPage = false,
				OnePageCheckoutEnabled = true,
				OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab = false,
				DisableBillingAddressCheckoutStep = false,
				DisableOrderCompletedPage = false,
				AttachPdfInvoiceToOrderPlacedEmail = false,
				AttachPdfInvoiceToOrderCompletedEmail = false,
				GeneratePdfInvoiceInCustomerLanguage = true,
				AttachPdfInvoiceToOrderPaidEmail = false,
				ReturnRequestsEnabled = true,
				ReturnRequestsAllowFiles = false,
				ReturnRequestsFileMaximumSize = 2048,
				NumberOfDaysReturnRequestAvailable = 365,
				MinimumOrderPlacementInterval = 30,
				ActivateGiftCardsAfterCompletingOrder = false,
				DeactivateGiftCardsAfterCancellingOrder = false,
				DeactivateGiftCardsAfterDeletingOrder = false,
				CompleteOrderWhenDelivered = true,
				CustomOrderNumberMask = "{ID}",
				ExportWithProducts = true
			});

			settingService.SaveSetting(new SecuritySettings
			{
				ForceSslForAllPages = false,
				EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
				AdminAreaAllowedIpAddresses = null,
				EnableXsrfProtectionForAdminArea = true,
				EnableXsrfProtectionForPublicStore = true,
				HoneypotEnabled = false,
				HoneypotInputName = "hpinput"
			});

			settingService.SaveSetting(new ShippingSettings
			{
				ActiveShippingRateComputationMethodSystemNames = new List<string> { "Shipping.FixedOrByWeight" },
				ActivePickupPointProviderSystemNames = new List<string> { "Pickup.PickupInStore" },
				ShipToSameAddress = true,
				AllowPickUpInStore = true,
				DisplayPickupPointsOnMap = false,
				UseWarehouseLocation = false,
				NotifyCustomerAboutShippingFromMultipleLocations = false,
				FreeShippingOverXEnabled = false,
				FreeShippingOverXValue = decimal.Zero,
				FreeShippingOverXIncludingTax = false,
				EstimateShippingEnabled = true,
				DisplayShipmentEventsToCustomers = false,
				DisplayShipmentEventsToStoreOwner = false,
				HideShippingTotal = false,
				ReturnValidOptionsIfThereAreAny = true,
				BypassShippingMethodSelectionIfOnlyOne = false,
				UseCubeRootMethod = true,
				ConsiderAssociatedProductsDimensions = true
			});

			settingService.SaveSetting(new PaymentSettings
			{
				ActivePaymentMethodSystemNames = new List<string>
					{
						"Payments.CheckMoneyOrder",
						"Payments.Manual",
						"Payments.PayInStore",
						"Payments.PurchaseOrder",
					},
				AllowRePostingPayments = true,
				BypassPaymentMethodSelectionIfOnlyOne = true,
				ShowPaymentMethodDescriptions = true,
				SkipPaymentInfoStepForRedirectionPaymentMethods = false,
				CancelRecurringPaymentsAfterFailedPayment = false
			});

			settingService.SaveSetting(new TaxSettings
			{
				TaxBasedOn = TaxBasedOn.BillingAddress,
				TaxBasedOnPickupPointAddress = false,
				TaxDisplayType = TaxDisplayType.ExcludingTax,
				ActiveTaxProviderSystemName = "Tax.FixedOrByCountryStateZip",
				DefaultTaxAddressId = 0,
				DisplayTaxSuffix = false,
				DisplayTaxRates = false,
				PricesIncludeTax = false,
				AllowCustomersToSelectTaxDisplayType = false,
				ForceTaxExclusionFromOrderSubtotal = false,
				DefaultTaxCategoryId = 0,
				HideZeroTax = false,
				HideTaxInOrderSummary = false,
				ShippingIsTaxable = false,
				ShippingPriceIncludesTax = false,
				ShippingTaxClassId = 0,
				PaymentMethodAdditionalFeeIsTaxable = false,
				PaymentMethodAdditionalFeeIncludesTax = false,
				PaymentMethodAdditionalFeeTaxClassId = 0,
				EuVatEnabled = false,
				EuVatShopCountryId = 0,
				EuVatAllowVatExemption = true,
				EuVatUseWebService = false,
				EuVatAssumeValid = false,
				EuVatEmailAdminWhenNewVatSubmitted = false,
				LogErrors = false
			});

			settingService.SaveSetting(new DateTimeSettings
			{
				DefaultStoreTimeZoneId = "",
				AllowCustomersToSetTimeZone = false
			});

			settingService.SaveSetting(new BlogSettings
			{
				Enabled = true,
				PostsPageSize = 10,
				AllowNotRegisteredUsersToLeaveComments = true,
				NotifyAboutNewBlogComments = false,
				NumberOfTags = 15,
				ShowHeaderRssUrl = false,
				BlogCommentsMustBeApproved = false,
				ShowBlogCommentsPerStore = false
			});
			settingService.SaveSetting(new NewsSettings
			{
				Enabled = true,
				AllowNotRegisteredUsersToLeaveComments = true,
				NotifyAboutNewNewsComments = false,
				ShowNewsOnMainPage = true,
				MainPageNewsCount = 3,
				NewsArchivePageSize = 10,
				ShowHeaderRssUrl = false,
				NewsCommentsMustBeApproved = false,
				ShowNewsCommentsPerStore = false
			});

			settingService.SaveSetting(new ForumSettings
			{
				ForumsEnabled = false,
				RelativeDateTimeFormattingEnabled = true,
				AllowCustomersToDeletePosts = false,
				AllowCustomersToEditPosts = false,
				AllowCustomersToManageSubscriptions = false,
				AllowGuestsToCreatePosts = false,
				AllowGuestsToCreateTopics = false,
				AllowPostVoting = true,
				MaxVotesPerDay = 30,
				TopicSubjectMaxLength = 450,
				PostMaxLength = 4000,
				StrippedTopicMaxLength = 45,
				TopicsPageSize = 10,
				PostsPageSize = 10,
				SearchResultsPageSize = 10,
				ActiveDiscussionsPageSize = 50,
				LatestCustomerPostsPageSize = 10,
				ShowCustomersPostCount = true,
				ForumEditor = EditorType.BBCodeEditor,
				SignaturesEnabled = true,
				AllowPrivateMessages = false,
				ShowAlertForPM = false,
				PrivateMessagesPageSize = 10,
				ForumSubscriptionsPageSize = 10,
				NotifyAboutPrivateMessages = false,
				PMSubjectMaxLength = 450,
				PMTextMaxLength = 4000,
				HomePageActiveDiscussionsTopicCount = 5,
				ActiveDiscussionsFeedEnabled = false,
				ActiveDiscussionsFeedCount = 25,
				ForumFeedsEnabled = false,
				ForumFeedCount = 10,
				ForumSearchTermMinimumLength = 3,
			});

			settingService.SaveSetting(new VendorSettings
			{
				DefaultVendorPageSizeOptions = "6, 3, 9",
				VendorsBlockItemsToDisplay = 0,
				ShowVendorOnProductDetailsPage = true,
				AllowCustomersToContactVendors = true,
				AllowCustomersToApplyForVendorAccount = true,
				AllowVendorsToEditInfo = false,
				NotifyStoreOwnerAboutVendorInformationChange = true,
				MaximumProductNumber = 3000,
				AllowVendorsToImportProducts = true
			});

			var eaGeneral = _emailAccountRepository.Table.FirstOrDefault();
			if (eaGeneral == null)
				throw new Exception("Default email account cannot be loaded");
			settingService.SaveSetting(new EmailAccountSettings
			{
				DefaultEmailAccountId = eaGeneral.Id
			});

			settingService.SaveSetting(new WidgetSettings
			{
				ActiveWidgetSystemNames = new List<string> { "Widgets.NivoSlider" },
			});
		}

		protected virtual void InstallCheckoutAttributes()
		{
			//var ca1 = new CheckoutAttribute
			//{
			//	Name = "Gift wrapping",
			//	IsRequired = true,
			//	ShippableProductRequired = true,
			//	AttributeControlType = AttributeControlType.DropdownList,
			//	DisplayOrder = 1,
			//};
			//ca1.CheckoutAttributeValues.Add(new CheckoutAttributeValue
			//{
			//	Name = "No",
			//	PriceAdjustment = 0,
			//	DisplayOrder = 1,
			//	IsPreSelected = true,
			//});
			//ca1.CheckoutAttributeValues.Add(new CheckoutAttributeValue
			//{
			//	Name = "Yes",
			//	PriceAdjustment = 10,
			//	DisplayOrder = 2,
			//});
			//var checkoutAttributes = new List<CheckoutAttribute>
			//					{
			//						ca1,
			//					};
			//_checkoutAttributeRepository.Insert(checkoutAttributes);
		}

		protected virtual void InstallSpecificationAttributes()
		{
			//var sa1 = new SpecificationAttribute
			//{
			//	Name = "Screensize",
			//	DisplayOrder = 1,
			//};
			//sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption
			//{
			//	Name = "13.0''",
			//	DisplayOrder = 2,
			//});
			//sa1.SpecificationAttributeOptions.Add(new SpecificationAttributeOption
			//{
			//	Name = "13.3''",
			//	DisplayOrder = 3,
			//});		

			//var sa5 = new SpecificationAttribute
			//{
			//	Name = "Color",
			//	DisplayOrder = 1,
			//};
			//sa5.SpecificationAttributeOptions.Add(new SpecificationAttributeOption
			//{
			//	Name = "Grey",
			//	DisplayOrder = 2,
			//	ColorSquaresRgb = "#8a97a8"
			//});
			//sa5.SpecificationAttributeOptions.Add(new SpecificationAttributeOption
			//{
			//	Name = "Red",
			//	DisplayOrder = 3,
			//	ColorSquaresRgb = "#8a374a"
			//});
			//sa5.SpecificationAttributeOptions.Add(new SpecificationAttributeOption
			//{
			//	Name = "Blue",
			//	DisplayOrder = 4,
			//	ColorSquaresRgb = "#47476f"
			//});
			//var specificationAttributes = new List<SpecificationAttribute>
			//					{
			//						sa1,
			//						sa2,
			//						sa3,
			//						sa4,
			//						sa5
			//					};
			//_specificationAttributeRepository.Insert(specificationAttributes);
		}

		protected virtual void InstallProductAttributes()
		{
			//var productAttributes = new List<ProductAttribute>
			//{
			//	new ProductAttribute
			//	{
			//		Name = "Color",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "Print",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "Custom Text",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "HDD",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "OS",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "Processor",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "RAM",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "Size",
			//	},
			//	new ProductAttribute
			//	{
			//		Name = "Software",
			//	},
			//};
			//_productAttributeRepository.Insert(productAttributes);
		}

		protected virtual void InstallCategories()
		{
			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();
			var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
			//pictures
			var pictureService = EngineContext.Current.Resolve<IPictureService>();
			var sampleImagesPath = CommonHelper.MapPath("~/Themes/NopElectro/content/images/categories/");

			var categoryTemplateInGridAndLines = _categoryTemplateRepository
				.Table.FirstOrDefault(pt => pt.Name == "Products in Grid or Lines");
			if (categoryTemplateInGridAndLines == null)
				throw new Exception("Category template cannot be loaded");
			//categories
			var allCategories = new List<Category>();
			var categoryComponents = new Category
			{
				Name = "Комплектующие",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Components.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Components")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
				ShowOnHomePage = true
			};
			allCategories.Add(categoryComponents);
			_categoryRepository.Insert(categoryComponents);
			_localizedEntityService.SaveLocalizedValue(categoryComponents, c => c.Name, "Комплектующие", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryComponents, c => c.Name, "Комплектуючі", languageUa.Id);

			#region Components subcategories

			var categoryVideoadapters = new Category
			{
				Name = "Видеокарты",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PriceRanges = "auto",
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Videoadapters.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Videoadapters")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryVideoadapters);
			_categoryRepository.Insert(categoryVideoadapters);
			_localizedEntityService.SaveLocalizedValue(categoryVideoadapters, c => c.Name, "Видеокарты", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryVideoadapters, c => c.Name, "Відеокарти", languageUa.Id);

			var categoryMotherboards = new Category
			{
				Name = "Материнские платы",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PriceRanges = "auto",
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Motherboards.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Motherboards")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 2,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMotherboards);
			_categoryRepository.Insert(categoryMotherboards);
			_localizedEntityService.SaveLocalizedValue(categoryMotherboards, c => c.Name, "Материнские платы", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMotherboards, c => c.Name, "Материнські плати", languageUa.Id);

			var categoryPowerSupplies = new Category
			{
				Name = "Блоки питания",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Powersupplies.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Powersupplies")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 3,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryPowerSupplies);
			_categoryRepository.Insert(categoryPowerSupplies);
			_localizedEntityService.SaveLocalizedValue(categoryPowerSupplies, c => c.Name, "Блоки питания", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryPowerSupplies, c => c.Name, "Блоки живлення", languageUa.Id);

			var categoryProcessors = new Category
			{
				Name = "Процессоры",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Processors.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Processors")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 4,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryProcessors);
			_categoryRepository.Insert(categoryProcessors);
			_localizedEntityService.SaveLocalizedValue(categoryProcessors, c => c.Name, "Процессоры", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryProcessors, c => c.Name, "Процесори", languageUa.Id);

			var categoryRAMs = new Category
			{
				Name = "Оперативная память",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "RAMs.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("RAMs")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 5,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryRAMs);
			_categoryRepository.Insert(categoryRAMs);
			_localizedEntityService.SaveLocalizedValue(categoryRAMs, c => c.Name, "Оперативная память", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryRAMs, c => c.Name, "Оперативна пам'ять", languageUa.Id);

			var categorySSDDrives = new Category
			{
				Name = "SSD диски",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "SSDDrives.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("SSDDrives")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 6,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categorySSDDrives);
			_categoryRepository.Insert(categorySSDDrives);
			_localizedEntityService.SaveLocalizedValue(categorySSDDrives, c => c.Name, "SSD диски", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categorySSDDrives, c => c.Name, "SSD диски", languageUa.Id);

			var categoryHDDDrives = new Category
			{
				Name = "HDD диски",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "HDDDrives.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("HDDDrives")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 7,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryHDDDrives);
			_categoryRepository.Insert(categoryHDDDrives);
			_localizedEntityService.SaveLocalizedValue(categoryHDDDrives, c => c.Name, "HDD диски", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryHDDDrives, c => c.Name, "HDD диски", languageUa.Id);

			var categoryAudioadapters = new Category
			{
				Name = "Звуковые карты",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Audioadapters.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Audioadapters")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 8,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAudioadapters);
			_categoryRepository.Insert(categoryAudioadapters);
			_localizedEntityService.SaveLocalizedValue(categoryAudioadapters, c => c.Name, "Звуковые карты", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAudioadapters, c => c.Name, "Звукові карти", languageUa.Id);

			var categoryCooling = new Category
			{
				Name = "Охлаждение",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Cooling.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Cooling")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 9,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryCooling);
			_categoryRepository.Insert(categoryCooling);
			_localizedEntityService.SaveLocalizedValue(categoryCooling, c => c.Name, "Охлаждение", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryCooling, c => c.Name, "Охолодження", languageUa.Id);

			var categoryFrames = new Category
			{
				Name = "Корпуса",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryComponents.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Frames.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Frames")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 10,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryFrames);
			_categoryRepository.Insert(categoryFrames);
			_localizedEntityService.SaveLocalizedValue(categoryFrames, c => c.Name, "Корпуса", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryFrames, c => c.Name, "Корпуси", languageUa.Id);

			#endregion

			var categoryMining = new Category
			{
				Name = "Майнинг",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Mining.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Mining")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 2,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
				ShowOnHomePage = true
			};
			allCategories.Add(categoryMining);
			_categoryRepository.Insert(categoryMining);
			_localizedEntityService.SaveLocalizedValue(categoryMining, c => c.Name, "Майнинг", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMining, c => c.Name, "Майнінг", languageUa.Id);

			#region Mining subcategories

			var categoryMiningVideoadapters = new Category
			{
				Name = "Видеокарты",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Videoadapters.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Videoadapters")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningVideoadapters);
			_categoryRepository.Insert(categoryMiningVideoadapters);
			_localizedEntityService.SaveLocalizedValue(categoryMiningVideoadapters, c => c.Name, "Видеокарты", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningVideoadapters, c => c.Name, "Відеокарти", languageUa.Id);

			var categoryMiningMotherboards = new Category
			{
				Name = "Материнские платы",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Motherboards.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Motherboards")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 2,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningMotherboards);
			_categoryRepository.Insert(categoryMiningMotherboards);
			_localizedEntityService.SaveLocalizedValue(categoryMiningMotherboards, c => c.Name, "Материнские платы", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningMotherboards, c => c.Name, "Материнські плати", languageUa.Id);

			var categoryMiningPowerSupplies = new Category
			{
				Name = "Блоки питания",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Powersupplies.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Powersupplies")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 3,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningPowerSupplies);
			_categoryRepository.Insert(categoryMiningPowerSupplies);
			_localizedEntityService.SaveLocalizedValue(categoryMiningPowerSupplies, c => c.Name, "Блоки питания", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningPowerSupplies, c => c.Name, "Блоки живлення", languageUa.Id);

			var categoryMiningProcessors = new Category
			{
				Name = "Процессоры",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Processors.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Processors")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 4,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningProcessors);
			_categoryRepository.Insert(categoryMiningProcessors);
			_localizedEntityService.SaveLocalizedValue(categoryMiningProcessors, c => c.Name, "Процессоры", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningProcessors, c => c.Name, "Процесори", languageUa.Id);

			var categoryMiningRAMs = new Category
			{
				Name = "Оперативная память",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "RAMs.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("RAMs")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 5,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningRAMs);
			_categoryRepository.Insert(categoryMiningRAMs);
			_localizedEntityService.SaveLocalizedValue(categoryMiningRAMs, c => c.Name, "Оперативная память", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningRAMs, c => c.Name, "Оперативна пам'ять", languageUa.Id);

			var categoryMiningSSDDrives = new Category
			{
				Name = "SSD диски",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "SSDDrives.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("SSDDrives")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 6,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningSSDDrives);
			_categoryRepository.Insert(categoryMiningSSDDrives);
			_localizedEntityService.SaveLocalizedValue(categoryMiningSSDDrives, c => c.Name, "SSD диски", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningSSDDrives, c => c.Name, "SSD диски", languageUa.Id);

			var categoryMiningRaizers = new Category
			{
				Name = "Райзеры",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Raizers.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Raizers")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 7,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningRaizers);
			_categoryRepository.Insert(categoryMiningRaizers);
			_localizedEntityService.SaveLocalizedValue(categoryMiningRaizers, c => c.Name, "Райзеры", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningRaizers, c => c.Name, "Райзери", languageUa.Id);

			var categoryMiningFrames = new Category
			{
				Name = "Корпуса",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Frames.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Frames")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 8,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningFrames);
			_categoryRepository.Insert(categoryMiningFrames);
			_localizedEntityService.SaveLocalizedValue(categoryMiningFrames, c => c.Name, "Корпуса", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningFrames, c => c.Name, "Корпуси", languageUa.Id);

			var categoryMiningCooling = new Category
			{
				Name = "Охлаждение",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				ParentCategoryId = categoryMining.Id,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Cooling.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Cooling")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 9,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryMiningCooling);
			_categoryRepository.Insert(categoryMiningCooling);
			_localizedEntityService.SaveLocalizedValue(categoryMiningCooling, c => c.Name, "Охлаждение", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryMiningCooling, c => c.Name, "Охолодження", languageUa.Id);

			#endregion

			var categoryAccessories = new Category
			{
				Name = "Аксессуары",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "Accessories.png"), MimeTypes.ImagePng, pictureService.GetPictureSeName("Accessories")).Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 3,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
				ShowOnHomePage = true
			};
			allCategories.Add(categoryAccessories);
			_categoryRepository.Insert(categoryAccessories);
			_localizedEntityService.SaveLocalizedValue(categoryAccessories, c => c.Name, "Аксессуары", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessories, c => c.Name, "Аксесуари", languageUa.Id);

			#region accessories subcategories

			var categoryAccessoriesAdapters = new Category
			{
				Name = "Переходники",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				ParentCategoryId = categoryAccessories.Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAccessoriesAdapters);
			_categoryRepository.Insert(categoryAccessoriesAdapters);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesAdapters, c => c.Name, "Переходники", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesAdapters, c => c.Name, "Перехідники", languageUa.Id);

			var categoryAccessoriesThermalGrease = new Category
			{
				Name = "Термопаста",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				ParentCategoryId = categoryAccessories.Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 2,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAccessoriesThermalGrease);
			_categoryRepository.Insert(categoryAccessoriesThermalGrease);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesThermalGrease, c => c.Name, "Термопаста", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesThermalGrease, c => c.Name, "Термопаста", languageUa.Id);

			var categoryAccessoriesCables = new Category
			{
				Name = "Кабели",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				ParentCategoryId = categoryAccessories.Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 3,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAccessoriesCables);
			_categoryRepository.Insert(categoryAccessoriesCables);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesCables, c => c.Name, "Кабели", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesCables, c => c.Name, "Кабелі", languageUa.Id);

			var categoryAccessoriesMats = new Category
			{
				Name = "Коврики",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				ParentCategoryId = categoryAccessories.Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 4,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAccessoriesMats);
			_categoryRepository.Insert(categoryAccessoriesMats);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesMats, c => c.Name, "Коврики", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesMats, c => c.Name, "Коврики", languageUa.Id);

			var categoryAccessoriesKeyboardsAndMice = new Category
			{
				Name = "Клавиатуры и мыши",
				CategoryTemplateId = categoryTemplateInGridAndLines.Id,
				PriceRanges = "auto",
				PageSize = 6,
				AllowCustomersToSelectPageSize = true,
				PageSizeOptions = "6, 3, 9",
				ParentCategoryId = categoryAccessories.Id,
				IncludeInTopMenu = true,
				Published = true,
				DisplayOrder = 5,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow
			};
			allCategories.Add(categoryAccessoriesKeyboardsAndMice);
			_categoryRepository.Insert(categoryAccessoriesKeyboardsAndMice);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesKeyboardsAndMice, c => c.Name, "Клавиатуры и мыши", languageRu.Id);
			_localizedEntityService.SaveLocalizedValue(categoryAccessoriesKeyboardsAndMice, c => c.Name, "Клавіатру та миші", languageUa.Id);

			#endregion

			//search engine names
			foreach (var category in allCategories)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = category.Id,
					EntityName = "Category",
					LanguageId = 0,
					IsActive = true,
					Slug = category.ValidateSeName("", category.Name, true)
				});
			}

			urlRecordService.SaveSlug(categoryAccessories, "Аксессуары", languageRu.Id);
			urlRecordService.SaveSlug(categoryAccessories, "Аксесуари", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningCooling, "Майнинг-Охлаждение", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningCooling, "Майнінг-Охолодження", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningFrames, "Майнинг-Корпуса", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningFrames, "Майнінг-Корпуси", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningRaizers, "Майнинг-Райзеры", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningRaizers, "Майнінг-Райзери", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningSSDDrives, "Майнинг-SSD-диски", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningSSDDrives, "Майнінг-SSD-диски", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningRAMs, "Майнинг-Оперативная-память", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningRAMs, "Майнінг-Оперативна-память", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningPowerSupplies, "Майнинг-Блоки-питания", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningPowerSupplies, "Майнінг-Блоки-живлення", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningPowerSupplies, "Майнинг-Блоки-питания", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningPowerSupplies, "Майнінг-Блоки-живлення", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningMotherboards, "Майнинг-Материнские-платы", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningMotherboards, "Майнінг-Материнські-плати", languageUa.Id);

			urlRecordService.SaveSlug(categoryMiningVideoadapters, "Майнинг-Видеокарты", languageRu.Id);
			urlRecordService.SaveSlug(categoryMiningVideoadapters, "Майнінг-Відеокарти", languageUa.Id);

			urlRecordService.SaveSlug(categoryMining, "Майнинг", languageRu.Id);
			urlRecordService.SaveSlug(categoryMining, "Майнінг", languageUa.Id);


			urlRecordService.SaveSlug(categoryComponents, "Комплектующие", languageRu.Id);
			urlRecordService.SaveSlug(categoryComponents, "Комплектуючі", languageUa.Id);

			urlRecordService.SaveSlug(categoryVideoadapters, "Видеокарты", languageRu.Id);
			urlRecordService.SaveSlug(categoryVideoadapters, "Відеокарти", languageUa.Id);

			urlRecordService.SaveSlug(categoryMotherboards, "Материнские платы", languageRu.Id);
			urlRecordService.SaveSlug(categoryMotherboards, "Материнські плати", languageUa.Id);

			urlRecordService.SaveSlug(categoryPowerSupplies, "Блоки питания", languageRu.Id);
			urlRecordService.SaveSlug(categoryPowerSupplies, "Блоки живлення", languageUa.Id);

			urlRecordService.SaveSlug(categoryProcessors, "Процессоры", languageRu.Id);
			urlRecordService.SaveSlug(categoryProcessors, "Процесори", languageUa.Id);

			urlRecordService.SaveSlug(categoryRAMs, "Оперативная-память", languageRu.Id);
			urlRecordService.SaveSlug(categoryRAMs, "Оперативна-память", languageUa.Id);

			urlRecordService.SaveSlug(categorySSDDrives, "SSD диски", languageRu.Id);
			urlRecordService.SaveSlug(categorySSDDrives, "SSD диски", languageUa.Id);

			urlRecordService.SaveSlug(categoryHDDDrives, "HDD диски", languageRu.Id);
			urlRecordService.SaveSlug(categoryHDDDrives, "HDD диски", languageUa.Id);

			urlRecordService.SaveSlug(categoryAudioadapters, "Звуковые карты", languageRu.Id);
			urlRecordService.SaveSlug(categoryAudioadapters, "Звукові карти", languageUa.Id);

			urlRecordService.SaveSlug(categoryCooling, "Охлаждение", languageRu.Id);
			urlRecordService.SaveSlug(categoryCooling, "Охолодження", languageUa.Id);

			urlRecordService.SaveSlug(categoryFrames, "Корпуса", languageRu.Id);
			urlRecordService.SaveSlug(categoryFrames, "Корпуси", languageUa.Id);
		}

		protected virtual void InstallManufacturers()
		{
			//var pictureService = EngineContext.Current.Resolve<IPictureService>();
			//var sampleImagesPath = CommonHelper.MapPath("~/content/samples/");

			//var manufacturerTemplateInGridAndLines =
			//	_manufacturerTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Products in Grid or Lines");
			//if (manufacturerTemplateInGridAndLines == null)
			//	throw new Exception("Manufacturer template cannot be loaded");

			//var allManufacturers = new List<Manufacturer>();
			//var manufacturerAsus = new Manufacturer
			//{
			//	Name = "Apple",
			//	ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
			//	PageSize = 6,
			//	AllowCustomersToSelectPageSize = true,
			//	PageSizeOptions = "6, 3, 9",
			//	Published = true,
			//	PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer_apple.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName("Apple")).Id,
			//	DisplayOrder = 1,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow
			//};
			//_manufacturerRepository.Insert(manufacturerAsus);
			//allManufacturers.Add(manufacturerAsus);


			//var manufacturerHp = new Manufacturer
			//{
			//	Name = "HP",
			//	ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
			//	PageSize = 6,
			//	AllowCustomersToSelectPageSize = true,
			//	PageSizeOptions = "6, 3, 9",
			//	Published = true,
			//	PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer_hp.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName("Hp")).Id,
			//	DisplayOrder = 5,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow
			//};
			//_manufacturerRepository.Insert(manufacturerHp);
			//allManufacturers.Add(manufacturerHp);


			//var manufacturerNike = new Manufacturer
			//{
			//	Name = "Nike",
			//	ManufacturerTemplateId = manufacturerTemplateInGridAndLines.Id,
			//	PageSize = 6,
			//	AllowCustomersToSelectPageSize = true,
			//	PageSizeOptions = "6, 3, 9",
			//	Published = true,
			//	PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "manufacturer_nike.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName("Nike")).Id,
			//	DisplayOrder = 5,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow
			//};
			//_manufacturerRepository.Insert(manufacturerNike);
			//allManufacturers.Add(manufacturerNike);

			////search engine names
			//foreach (var manufacturer in allManufacturers)
			//{
			//	_urlRecordRepository.Insert(new UrlRecord
			//	{
			//		EntityId = manufacturer.Id,
			//		EntityName = "Manufacturer",
			//		LanguageId = 0,
			//		IsActive = true,
			//		Slug = manufacturer.ValidateSeName("", manufacturer.Name, true)
			//	});
			//}
		}

		protected virtual void InstallProducts(string defaultUserEmail)
		{
			var productTemplateSimple = _productTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Simple product");
			if (productTemplateSimple == null)
				throw new Exception("Simple product template could not be loaded");
			var productTemplateGrouped = _productTemplateRepository.Table.FirstOrDefault(pt => pt.Name == "Grouped product (with variants)");
			if (productTemplateGrouped == null)
				throw new Exception("Grouped product template could not be loaded");

			////delivery date
			//var deliveryDate = _deliveryDateRepository.Table.FirstOrDefault();
			//if (deliveryDate == null)
			//	throw new Exception("No default deliveryDate could be loaded");

			////product availability range
			//var productAvailabilityRange = _productAvailabilityRangeRepository.Table.FirstOrDefault();
			//if (productAvailabilityRange == null)
			//	throw new Exception("No default product availability range could be loaded");

			////default customer/user
			//var defaultCustomer = _customerRepository.Table.FirstOrDefault(x => x.Email == defaultUserEmail);
			//if (defaultCustomer == null)
			//	throw new Exception("Cannot load default customer");

			////default store
			//var defaultStore = _storeRepository.Table.FirstOrDefault();
			//if (defaultStore == null)
			//	throw new Exception("No default store could be loaded");


			////pictures
			//var pictureService = EngineContext.Current.Resolve<IPictureService>();
			//var sampleImagesPath = CommonHelper.MapPath("~/content/samples/");

			////downloads
			//var downloadService = EngineContext.Current.Resolve<IDownloadService>();
			//var sampleDownloadsPath = CommonHelper.MapPath("~/content/samples/");

			////products
			//var allProducts = new List<Product>();

			//#region Desktops


			//var productBuildComputer = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Build your own computer",
			//	Sku = "COMP_CUST",
			//	ShortDescription = "Build it",
			//	FullDescription = "<p>Fight back against cluttered workspaces with the stylish IBM zBC12 All-in-One desktop PC, featuring powerful computing resources and a stunning 20.1-inch widescreen display with stunning XBRITE-HiColor LCD technology. The black IBM zBC12 has a built-in microphone and MOTION EYE camera with face-tracking technology that allows for easy communication with friends and family. And it has a built-in DVD burner and Sony's Movie Store software so you can create a digital entertainment library for personal viewing at your convenience. Easy to setup and even easier to use, this JS-series All-in-One includes an elegantly designed keyboard and a USB mouse.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "build-your-own-computer",
			//	AllowCustomerReviews = true,
			//	Price = 1200M,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	ShowOnHomePage = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Processor"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "2.2 GHz Intel Pentium Dual-Core E2200",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "2.5 GHz Intel Pentium Dual-Core E2200",
			//					IsPreSelected = true,
			//					PriceAdjustment = 15,
			//					DisplayOrder = 2,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "RAM"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "2 GB",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "4GB",
			//					PriceAdjustment = 20,
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "8GB",
			//					PriceAdjustment = 60,
			//					DisplayOrder = 3,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "HDD"),
			//			AttributeControlType = AttributeControlType.RadioList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "320 GB",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "400 GB",
			//					PriceAdjustment = 100,
			//					DisplayOrder = 2,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "OS"),
			//			AttributeControlType = AttributeControlType.RadioList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Vista Home",
			//					PriceAdjustment = 50,
			//					IsPreSelected = true,
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Vista Premium",
			//					PriceAdjustment = 60,
			//					DisplayOrder = 2,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Software"),
			//			AttributeControlType = AttributeControlType.Checkboxes,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Microsoft Office",
			//					PriceAdjustment = 50,
			//					IsPreSelected = true,
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Acrobat Reader",
			//					PriceAdjustment = 10,
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Total Commander",
			//					PriceAdjustment = 5,
			//					DisplayOrder = 2,
			//				}
			//			}
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productBuildComputer);
			//productBuildComputer.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Desktops_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productBuildComputer.Name)),
			//	DisplayOrder = 1,
			//});
			//productBuildComputer.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Desktops_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productBuildComputer.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productBuildComputer);





			//var productDigitalStorm = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Digital Storm VANQUISH 3 Custom Performance PC",
			//	Sku = "DS_VA3_PC",
			//	ShortDescription = "Digital Storm Vanquish 3 Desktop PC",
			//	FullDescription = "<p>Blow the doors off today’s most demanding games with maximum detail, speed, and power for an immersive gaming experience without breaking the bank.</p><p>Stay ahead of the competition, VANQUISH 3 is fully equipped to easily handle future upgrades, keeping your system on the cutting edge for years to come.</p><p>Each system is put through an extensive stress test, ensuring you experience zero bottlenecks and get the maximum performance from your hardware.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "compaq-presario-sr1519x-pentium-4-desktop-pc-with-cdrw",
			//	AllowCustomerReviews = true,
			//	Price = 1259M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productDigitalStorm);
			//productDigitalStorm.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DigitalStorm.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productDigitalStorm.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productDigitalStorm);





			//var productLenovoIdeaCentre = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Lenovo IdeaCentre 600 All-in-One PC",
			//	Sku = "LE_IC_600",
			//	ShortDescription = "",
			//	FullDescription = "<p>The A600 features a 21.5in screen, DVD or optional Blu-Ray drive, support for the full beans 1920 x 1080 HD, Dolby Home Cinema certification and an optional hybrid analogue/digital TV tuner.</p><p>Connectivity is handled by 802.11a/b/g - 802.11n is optional - and an ethernet port. You also get four USB ports, a Firewire slot, a six-in-one card reader and a 1.3- or two-megapixel webcam.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "hp-iq506-touchsmart-desktop-pc",
			//	AllowCustomerReviews = true,
			//	Price = 500M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productLenovoIdeaCentre);
			//productLenovoIdeaCentre.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LenovoIdeaCentre.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productLenovoIdeaCentre.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productLenovoIdeaCentre);




			//#endregion

			//#region Notebooks

			//var productAppleMacBookPro = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Apple MacBook Pro 13-inch",
			//	Sku = "AP_MBP_13",
			//	ShortDescription = "A groundbreaking Retina display. A new force-sensing trackpad. All-flash architecture. Powerful dual-core and quad-core Intel processors. Together, these features take the notebook to a new level of performance. And they will do the same for you in everything you create.",
			//	FullDescription = "<p>With fifth-generation Intel Core processors, the latest graphics, and faster flash storage, the incredibly advanced MacBook Pro with Retina display moves even further ahead in performance and battery life.* *Compared with the previous generation.</p><p>Retina display with 2560-by-1600 resolution</p><p>Fifth-generation dual-core Intel Core i5 processor</p><p>Intel Iris Graphics</p><p>Up to 9 hours of battery life1</p><p>Faster flash storage2</p><p>802.11ac Wi-Fi</p><p>Two Thunderbolt 2 ports for connecting high-performance devices and transferring data at lightning speed</p><p>Two USB 3 ports (compatible with USB 2 devices) and HDMI</p><p>FaceTime HD camera</p><p>Pages, Numbers, Keynote, iPhoto, iMovie, GarageBand included</p><p>OS X, the world's most advanced desktop operating system</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "asus-eee-pc-1000ha-10-inch-netbook",
			//	AllowCustomerReviews = true,
			//	Price = 1800M,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	Weight = 3,
			//	Length = 3,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 2,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	ShowOnHomePage = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "Apple"),
			//			DisplayOrder = 2,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "13.0''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i5")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "4 GB")
			//		}
			//		//new ProductSpecificationAttribute
			//		//{
			//		//    AllowFiltering = false,
			//		//    ShowOnProductPage = true,
			//		//    DisplayOrder = 4,
			//		//    SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "160 GB")
			//		//}
			//	}
			//};
			//allProducts.Add(productAppleMacBookPro);
			//productAppleMacBookPro.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_macbook_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productAppleMacBookPro.Name)),
			//	DisplayOrder = 1,
			//});
			//productAppleMacBookPro.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_macbook_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productAppleMacBookPro.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productAppleMacBookPro);





			//var productAsusN551JK = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Asus N551JK-XO076H Laptop",
			//	Sku = "AS_551_LP",
			//	ShortDescription = "Laptop Asus N551JK Intel Core i7-4710HQ 2.5 GHz, RAM 16GB, HDD 1TB, Video NVidia GTX 850M 4GB, BluRay, 15.6, Full HD, Win 8.1",
			//	FullDescription = "<p>The ASUS N550JX combines cutting-edge audio and visual technology to deliver an unsurpassed multimedia experience. A full HD wide-view IPS panel is tailor-made for watching movies and the intuitive touchscreen makes for easy, seamless navigation. ASUS has paired the N550JX’s impressive display with SonicMaster Premium, co-developed with Bang & Olufsen ICEpower® audio experts, for true surround sound. A quad-speaker array and external subwoofer combine for distinct vocals and a low bass that you can feel.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "asus-eee-pc-900ha-89-inch-netbook-black",
			//	AllowCustomerReviews = true,
			//	Price = 1500M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		 new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "15.6''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i7")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "16 GB")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 4,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "1 TB")
			//		}
			//	}
			//};
			//allProducts.Add(productAsusN551JK);
			//productAsusN551JK.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_asuspc_N551JK.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productAsusN551JK.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productAsusN551JK);





			//var productSamsungSeries = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Samsung Series 9 NP900X4C Premium Ultrabook",
			//	Sku = "SM_900_PU",
			//	ShortDescription = "Samsung Series 9 NP900X4C-A06US 15-Inch Ultrabook (1.70 GHz Intel Core i5-3317U Processor, 8GB DDR3, 128GB SSD, Windows 8) Ash Black",
			//	FullDescription = "<p>Designed with mobility in mind, Samsung's durable, ultra premium, lightweight Series 9 laptop (model NP900X4C-A01US) offers mobile professionals and power users a sophisticated laptop equally suited for work and entertainment. Featuring a minimalist look that is both simple and sophisticated, its polished aluminum uni-body design offers an iconic look and feel that pushes the envelope with an edge just 0.58 inches thin. This Series 9 laptop also includes a brilliant 15-inch SuperBright Plus display with HD+ technology, 128 GB Solid State Drive (SSD), 8 GB of system memory, and up to 10 hours of battery life.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "hp-pavilion-artist-edition-dv2890nr-141-inch-laptop",
			//	AllowCustomerReviews = true,
			//	Price = 1590M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	//ShowOnHomePage = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "15.0''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i5")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "8 GB")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 4,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "128 GB")
			//		}
			//	}
			//};
			//allProducts.Add(productSamsungSeries);
			//productSamsungSeries.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_SamsungNP900X4C.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productSamsungSeries.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productSamsungSeries);





			//var productHpSpectre = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "HP Spectre XT Pro UltraBook",
			//	Sku = "HP_SPX_UB",
			//	ShortDescription = "HP Spectre XT Pro UltraBook / Intel Core i5-2467M / 13.3 / 4GB / 128GB / Windows 7 Professional / Laptop",
			//	FullDescription = "<p>Introducing HP ENVY Spectre XT, the Ultrabook designed for those who want style without sacrificing substance. It's sleek. It's thin. And with Intel. Corer i5 processor and premium materials, it's designed to go anywhere from the bistro to the boardroom, it's unlike anything you've ever seen from HP.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "hp-pavilion-elite-m9150f-desktop-pc",
			//	AllowCustomerReviews = true,
			//	Price = 1350M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "HP"),
			//			DisplayOrder = 3,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "13.3''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i5")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "4 GB")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 4,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "128 GB")
			//		}
			//	}
			//};
			//allProducts.Add(productHpSpectre);
			//productHpSpectre.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HPSpectreXT_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHpSpectre.Name)),
			//	DisplayOrder = 1,
			//});
			//productHpSpectre.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HPSpectreXT_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHpSpectre.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productHpSpectre);



			//var productHpEnvy = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "HP Envy 6-1180ca 15.6-Inch Sleekbook",
			//	Sku = "HP_ESB_15",
			//	ShortDescription = "HP ENVY 6-1202ea Ultrabook Beats Audio, 3rd generation Intel® CoreTM i7-3517U processor, 8GB RAM, 500GB HDD, Microsoft Windows 8, AMD Radeon HD 8750M (2 GB DDR3 dedicated)",
			//	FullDescription = "The UltrabookTM that's up for anything. Thin and light, the HP ENVY is the large screen UltrabookTM with Beats AudioTM. With a soft-touch base that makes it easy to grab and go, it's a laptop that's up for anything.<br /><br /><b>Features</b><br /><br />- Windows 8 or other operating systems available<br /><br /><b>Top performance. Stylish design. Take notice.</b><br /><br />- At just 19.8 mm (0.78 in) thin, the HP ENVY UltrabookTM is slim and light enough to take anywhere. It's the laptop that gets you noticed with the power to get it done.<br />- With an eye-catching metal design, it's a laptop that you want to carry with you. The soft-touch, slip-resistant base gives you the confidence to carry it with ease.<br /><br /><b>More entertaining. More gaming. More fun.</b><br /><br />- Own the UltrabookTM with Beats AudioTM, dual speakers, a subwoofer, and an awesome display. Your music, movies and photo slideshows will always look and sound their best.<br />- Tons of video memory let you experience incredible gaming and multimedia without slowing down. Create and edit videos in a flash. And enjoy more of what you love to the fullest.<br />- The HP ENVY UltrabookTM is loaded with the ports you'd expect on a world-class laptop, but on a Sleekbook instead. Like HDMI, USB, RJ-45, and a headphone jack. You get all the right connections without compromising size.<br /><br /><b>Only from HP.</b><br /><br />- Life heats up. That's why there's HP CoolSense technology, which automatically adjusts your notebook's temperature based on usage and conditions. It stays cool. You stay comfortable.<br />- With HP ProtectSmart, your notebook's data stays safe from accidental bumps and bruises. It senses motion and plans ahead, stopping your hard drive and protecting your entire digital life.<br />- Keep playing even in dimly lit rooms or on red eye flights. The optional backlit keyboard[1] is full-size so you don't compromise comfort. Backlit keyboard. Another bright idea.<br /><br />",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "hp-pavilion-g60-230us-160-inch-laptop",
			//	AllowCustomerReviews = true,
			//	Price = 1460M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "HP"),
			//			DisplayOrder = 4,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "15.6''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i7")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "8 GB")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 4,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "500 GB")
			//		}
			//	}
			//};
			//allProducts.Add(productHpEnvy);
			//productHpEnvy.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HpEnvy6.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHpEnvy.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productHpEnvy);





			//var productLenovoThinkpad = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Lenovo Thinkpad X1 Carbon Laptop",
			//	Sku = "LE_TX1_CL",
			//	ShortDescription = "Lenovo Thinkpad X1 Carbon Touch Intel Core i7 14 Ultrabook",
			//	FullDescription = "<p>The X1 Carbon brings a new level of quality to the ThinkPad legacy of high standards and innovation. It starts with the durable, carbon fiber-reinforced roll cage, making for the best Ultrabook construction available, and adds a host of other new features on top of the old favorites. Because for 20 years, we haven't stopped innovating. And you shouldn't stop benefiting from that.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "toshiba-satellite-a305-s6908-154-inch-laptop",
			//	AllowCustomerReviews = true,
			//	Price = 1360M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//	   new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = false,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Screensize").SpecificationAttributeOptions.Single(sao => sao.Name == "14.0''")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = true,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "CPU Type").SpecificationAttributeOptions.Single(sao => sao.Name == "Intel Core i7")
			//		}
			//		//new ProductSpecificationAttribute
			//		//{
			//		//    AllowFiltering = true,
			//		//    ShowOnProductPage = true,
			//		//    DisplayOrder = 3,
			//		//    SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Memory").SpecificationAttributeOptions.Single(sao => sao.Name == "1 GB")
			//		//},
			//		//new ProductSpecificationAttribute
			//		//{
			//		//    AllowFiltering = false,
			//		//    ShowOnProductPage = true,
			//		//    DisplayOrder = 4,
			//		//    SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Hardrive").SpecificationAttributeOptions.Single(sao => sao.Name == "250 GB")
			//		//}
			//	}
			//};
			//allProducts.Add(productLenovoThinkpad);
			//productLenovoThinkpad.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LenovoThinkpad.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productLenovoThinkpad.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productLenovoThinkpad);

			//#endregion

			//#region Software


			//var productAdobePhotoshop = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Adobe Photoshop CS4",
			//	Sku = "AD_CS4_PH",
			//	ShortDescription = "Easily find and view all your photos",
			//	FullDescription = "<p>Adobe Photoshop CS4 software combines power and simplicity so you can make ordinary photos extraordinary; tell engaging stories in beautiful, personalized creations for print and web; and easily find and view all your photos. New Photoshop.com membership* works with Photoshop CS4 so you can protect your photos with automatic online backup and 2 GB of storage; view your photos anywhere you are; and share your photos in fun, interactive ways with invitation-only Online Albums.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "adobe-photoshop-elements-7",
			//	AllowCustomerReviews = true,
			//	Price = 75M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Software"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productAdobePhotoshop);
			//productAdobePhotoshop.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_AdobePhotoshop.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productAdobePhotoshop.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productAdobePhotoshop);






			//var productWindows8Pro = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Windows 8 Pro",
			//	Sku = "MS_WIN_8P",
			//	ShortDescription = "Windows 8 is a Microsoft operating system that was released in 2012 as part of the company's Windows NT OS family. ",
			//	FullDescription = "<p>Windows 8 Pro is comparable to Windows 7 Professional and Ultimate and is targeted towards enthusiasts and business users; it includes all the features of Windows 8. Additional features include the ability to receive Remote Desktop connections, the ability to participate in a Windows Server domain, Encrypting File System, Hyper-V, and Virtual Hard Disk Booting, Group Policy as well as BitLocker and BitLocker To Go. Windows Media Center functionality is available only for Windows 8 Pro as a separate software package.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "corel-paint-shop-pro-photo-x2",
			//	AllowCustomerReviews = true,
			//	Price = 65M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Software"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productWindows8Pro);
			//productWindows8Pro.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Windows8.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productWindows8Pro.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productWindows8Pro);





			//var productSoundForge = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Sound Forge Pro 11 (recurring)",
			//	Sku = "SF_PRO_11",
			//	ShortDescription = "Advanced audio waveform editor.",
			//	FullDescription = "<p>Sound Forge™ Pro is the application of choice for a generation of creative and prolific artists, producers, and editors. Record audio quickly on a rock-solid platform, address sophisticated audio processing tasks with surgical precision, and render top-notch master files with ease. New features include one-touch recording, metering for the new critical standards, more repair and restoration tools, and exclusive round-trip interoperability with SpectraLayers Pro. Taken together, these enhancements make this edition of Sound Forge Pro the deepest and most advanced audio editing platform available.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "major-league-baseball-2k9",
			//	IsRecurring = true,
			//	RecurringCycleLength = 30,
			//	RecurringCyclePeriod = RecurringProductCyclePeriod.Months,
			//	RecurringTotalCycles = 12,
			//	AllowCustomerReviews = true,
			//	Price = 54.99M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Software"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productSoundForge);
			//productSoundForge.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_SoundForge.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productSoundForge.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productSoundForge);




			//#endregion

			//#region Camera, Photo


			////this one is a grouped product with two associated ones
			//var productNikonD5500DSLR = new Product
			//{
			//	ProductType = ProductType.GroupedProduct,
			//	VisibleIndividually = true,
			//	Name = "Nikon D5500 DSLR",
			//	Sku = "N5500DS_0",
			//	ShortDescription = "Slim, lightweight Nikon D5500 packs a vari-angle touchscreen",
			//	FullDescription = "Nikon has announced its latest DSLR, the D5500. A lightweight, compact DX-format camera with a 24.2MP sensor, it’s the first of its type to offer a vari-angle touchscreen. The D5500 replaces the D5300 in Nikon’s range, and while it offers much the same features the company says it’s a much slimmer and lighter prospect. There’s a deep grip for easier handling and built-in Wi-Fi that lets you transfer and share shots via your phone or tablet.",
			//	ProductTemplateId = productTemplateGrouped.Id,
			//	//SeName = "canon-digital-slr-camera",
			//	AllowCustomerReviews = true,
			//	Published = true,
			//	Price = 670M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Camera & photo"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productNikonD5500DSLR);
			//productNikonD5500DSLR.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikonCamera_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productNikonD5500DSLR.Name)),
			//	DisplayOrder = 1,
			//});
			//productNikonD5500DSLR.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikonCamera_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productNikonD5500DSLR.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productNikonD5500DSLR);
			//var productNikonD5500DSLR_associated_1 = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = false, //hide this products
			//	ParentGroupedProductId = productNikonD5500DSLR.Id,
			//	Name = "Nikon D5500 DSLR - Black",
			//	Sku = "N5500DS_B",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "canon-digital-slr-camera-black",
			//	AllowCustomerReviews = true,
			//	Published = true,
			//	Price = 670M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow
			//};
			//allProducts.Add(productNikonD5500DSLR_associated_1);
			//productNikonD5500DSLR_associated_1.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikonCamera_black.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName("Canon Digital SLR Camera - Black")),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNikonD5500DSLR_associated_1);
			//var productNikonD5500DSLR_associated_2 = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = false, //hide this products
			//	ParentGroupedProductId = productNikonD5500DSLR.Id,
			//	Name = "Nikon D5500 DSLR - Red",
			//	Sku = "N5500DS_R",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "canon-digital-slr-camera-silver",
			//	AllowCustomerReviews = true,
			//	Published = true,
			//	Price = 630M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow
			//};
			//allProducts.Add(productNikonD5500DSLR_associated_2);
			//productNikonD5500DSLR_associated_2.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikonCamera_red.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName("Canon Digital SLR Camera - Silver")),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNikonD5500DSLR_associated_2);





			//var productLeica = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Leica T Mirrorless Digital Camera",
			//	Sku = "LT_MIR_DC",
			//	ShortDescription = "Leica T (Typ 701) Silver",
			//	FullDescription = "<p>The new Leica T offers a minimalist design that's crafted from a single block of aluminum.  Made in Germany and assembled by hand, this 16.3 effective mega pixel camera is easy to use.  With a massive 3.7 TFT LCD intuitive touch screen control, the user is able to configure and save their own menu system.  The Leica T has outstanding image quality and also has 16GB of built in memory.  This is Leica's first system camera to use Wi-Fi.  Add the T-App to your portable iOS device and be able to transfer and share your images (free download from the Apple App Store)</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "canon-vixia-hf100-camcorder",
			//	AllowCustomerReviews = true,
			//	Price = 530M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Camera & photo"),
			//			DisplayOrder = 3,
			//		}
			//	}
			//};
			//allProducts.Add(productLeica);
			//productLeica.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeicaT.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productLeica.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productLeica);






			//var productAppleICam = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Apple iCam",
			//	Sku = "APPLE_CAM",
			//	ShortDescription = "Photography becomes smart",
			//	FullDescription = "<p>A few months ago we featured the amazing WVIL camera, by many considered the future of digital photography. This is another very good looking concept, iCam is the vision of Italian designer Antonio DeRosa, the idea is to have a device that attaches to the iPhone 5, which then allows the user to have a camera with interchangeable lenses. The device would also feature a front-touch screen and a projector. Would be great if apple picked up on this and made it reality.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "panasonic-hdc-sdt750k-high-definition-3d-camcorder",
			//	AllowCustomerReviews = true,
			//	Price = 1300M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Camera & photo"),
			//			DisplayOrder = 2,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "Apple"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productAppleICam);
			//productAppleICam.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_iCam.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productAppleICam.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productAppleICam);




			//#endregion

			//#region Cell Phone

			//var productHtcOne = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "HTC One M8 Android L 5.0 Lollipop",
			//	Sku = "M8_HTC_5L",
			//	ShortDescription = "HTC - One (M8) 4G LTE Cell Phone with 32GB Memory - Gunmetal (Sprint)",
			//	FullDescription = "<p><b>HTC One (M8) Cell Phone for Sprint:</b> With its brushed-metal design and wrap-around unibody frame, the HTC One (M8) is designed to fit beautifully in your hand. It's fun to use with amped up sound and a large Full HD touch screen, and intuitive gesture controls make it seem like your phone almost knows what you need before you do. <br /><br />Sprint Easy Pay option available in store.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "blackberry-bold-9000-phone-black-att",
			//	AllowCustomerReviews = true,
			//	Price = 245M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	ShowOnHomePage = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Cell phones"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productHtcOne);
			//productHtcOne.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HTC_One_M8.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHtcOne.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productHtcOne);






			//var productHtcOneMini = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "HTC One Mini Blue",
			//	Sku = "OM_HTC_BL",
			//	ShortDescription = "HTC One and HTC One Mini now available in bright blue hue",
			//	FullDescription = "<p>HTC One mini smartphone with 4.30-inch 720x1280 display powered by 1.4GHz processor alongside 1GB RAM and 4-Ultrapixel rear camera.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "samsung-rugby-a837-phone-black-att",
			//	AllowCustomerReviews = true,
			//	Price = 100M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Cell phones"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productHtcOneMini);
			//productHtcOneMini.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HTC_One_Mini_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHtcOneMini.Name)),
			//	DisplayOrder = 1,
			//});
			//productHtcOneMini.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HTC_One_Mini_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productHtcOneMini.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productHtcOneMini);






			//var productNokiaLumia = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Nokia Lumia 1020",
			//	Sku = "N_1020_LU",
			//	ShortDescription = "Nokia Lumia 1020 4G Cell Phone (Unlocked)",
			//	FullDescription = "<p>Capture special moments for friends and family with this Nokia Lumia 1020 32GB WHITE cell phone that features an easy-to-use 41.0MP rear-facing camera and a 1.2MP front-facing camera. The AMOLED touch screen offers 768 x 1280 resolution for crisp visuals.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "sony-dcr-sr85-1mp-60gb-hard-drive-handycam-camcorder",
			//	AllowCustomerReviews = true,
			//	Price = 349M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Cell phones"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productNokiaLumia);
			//productNokiaLumia.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Lumia1020.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productNokiaLumia.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNokiaLumia);


			//#endregion

			//#region Others



			//var productBeatsPill = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Beats Pill 2.0 Wireless Speaker",
			//	Sku = "BP_20_WSP",
			//	ShortDescription = "<b>Pill 2.0 Portable Bluetooth Speaker (1-Piece):</b> Watch your favorite movies and listen to music with striking sound quality. This lightweight, portable speaker is easy to take with you as you travel to any destination, keeping you entertained wherever you are. ",
			//	FullDescription = "<ul><li>Pair and play with your Bluetooth® device with 30 foot range</li><li>Built-in speakerphone</li><li>7 hour rechargeable battery</li><li>Power your other devices with USB charge out</li><li>Tap two Beats Pills™ together for twice the sound with Beats Bond™</li></ul>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "acer-aspire-one-89-mini-notebook-case-black",
			//	AllowCustomerReviews = true,
			//	Price = 79.99M,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	TierPrices =
			//	{
			//		new TierPrice
			//		{
			//			Quantity = 2,
			//			Price = 19
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 5,
			//			Price = 17
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 10,
			//			Price = 15,
			//			StartDateTimeUtc = DateTime.UtcNow.AddDays(-7),
			//			EndDateTimeUtc = DateTime.UtcNow.AddDays(7)
			//		}
			//	},
			//	HasTierPrices = true,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Others"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productBeatsPill);
			//productBeatsPill.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_PillBeats_1.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productBeatsPill.Name)),
			//	DisplayOrder = 1,
			//});
			//productBeatsPill.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_PillBeats_2.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productBeatsPill.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productBeatsPill);





			//var productUniversalTabletCover = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Universal 7-8 Inch Tablet Cover",
			//	Sku = "TC_78I_UN",
			//	ShortDescription = "Universal protection for 7-inch & 8-inch tablets",
			//	FullDescription = "<p>Made of durable polyurethane, our Universal Cover is slim, lightweight, and strong, with protective corners that stretch to hold most 7 and 8-inch tablets securely. This tough case helps protects your tablet from bumps, scuffs, and dings.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "apc-back-ups-rs-800va-ups-800-va-ups-battery-lead-acid-br800blk",
			//	AllowCustomerReviews = true,
			//	Price = 39M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Others"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productUniversalTabletCover);
			//productUniversalTabletCover.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_TabletCover.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productUniversalTabletCover.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productUniversalTabletCover);




			//var productPortableSoundSpeakers = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Portable Sound Speakers",
			//	Sku = "PT_SPK_SN",
			//	ShortDescription = "Universall portable sound speakers",
			//	FullDescription = "<p>Your phone cut the cord, now it's time for you to set your music free and buy a Bluetooth speaker. Thankfully, there's one suited for everyone out there.</p><p>Some Bluetooth speakers excel at packing in as much functionality as the unit can handle while keeping the price down. Other speakers shuck excess functionality in favor of premium build materials instead. Whatever path you choose to go down, you'll be greeted with many options to suit your personal tastes.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "microsoft-bluetooth-notebook-mouse-5000-macwindows",
			//	AllowCustomerReviews = true,
			//	Price = 37M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Electronics & Software").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Others"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productPortableSoundSpeakers);
			//productPortableSoundSpeakers.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Speakers.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productPortableSoundSpeakers.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productPortableSoundSpeakers);


			//#endregion

			//#region Shoes


			//var productNikeFloral = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Nike Floral Roshe Customized Running Shoes",
			//	Sku = "NK_FRC_RS",
			//	ShortDescription = "When you ran across these shoes, you will immediately fell in love and needed a pair of these customized beauties.",
			//	FullDescription = "<p>Each Rosh Run is personalized and exclusive, handmade in our workshop Custom. Run Your Rosh creations born from the hand of an artist specialized in sneakers, more than 10 years of experience.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "adidas-womens-supernova-csh-7-running-shoe",
			//	AllowCustomerReviews = true,
			//	Price = 40M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Size"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "8",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "9",
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "10",
			//					DisplayOrder = 3,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "11",
			//					DisplayOrder = 4,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Color"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "White/Blue",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "White/Black",
			//					DisplayOrder = 2,
			//				},
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Print"),
			//			AttributeControlType = AttributeControlType.ImageSquares,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Natural",
			//					DisplayOrder = 1,
			//					ImageSquaresPictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "p_attribute_print_2.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName("Natural Print")).Id,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Fresh",
			//					DisplayOrder = 2,
			//					ImageSquaresPictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "p_attribute_print_1.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName("Fresh Print")).Id,
			//				},
			//			}
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Shoes"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "Nike"),
			//			DisplayOrder = 2,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = false,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption =
			//				_specificationAttributeRepository.Table.Single(sa => sa.Name == "Color")
			//					.SpecificationAttributeOptions.Single(sao => sao.Name == "Grey")
			//		}
			//	}
			//};
			//allProducts.Add(productNikeFloral);
			//productNikeFloral.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikeFloralShoe_1.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productNikeFloral.Name)),
			//	DisplayOrder = 1,
			//});
			//productNikeFloral.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikeFloralShoe_2.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productNikeFloral.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productNikeFloral);

			//productNikeFloral.ProductAttributeMappings.First(x => x.ProductAttribute.Name == "Print").ProductAttributeValues.First(x => x.Name == "Natural").PictureId = productNikeFloral.ProductPictures.ElementAt(0).PictureId;
			//productNikeFloral.ProductAttributeMappings.First(x => x.ProductAttribute.Name == "Print").ProductAttributeValues.First(x => x.Name == "Fresh").PictureId = productNikeFloral.ProductPictures.ElementAt(1).PictureId;
			//_productRepository.Update(productNikeFloral);



			//var productAdidas = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "adidas Consortium Campus 80s Running Shoes",
			//	Sku = "AD_C80_RS",
			//	ShortDescription = "adidas Consortium Campus 80s Primeknit Light Maroon/Running Shoes",
			//	FullDescription = "<p>One of three colorways of the adidas Consortium Campus 80s Primeknit set to drop alongside each other. This pair comes in light maroon and running white. Featuring a maroon-based primeknit upper with white accents. A limited release, look out for these at select adidas Consortium accounts worldwide.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "etnies-mens-digit-sneaker",
			//	AllowCustomerReviews = true,
			//	Price = 27.56M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	//ShowOnHomePage = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Size"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "8",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "9",
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "10",
			//					DisplayOrder = 3,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "11",
			//					DisplayOrder = 4,
			//				}
			//			}
			//		},
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Color"),
			//			AttributeControlType = AttributeControlType.ColorSquares,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Red",
			//					IsPreSelected = true,
			//					ColorSquaresRgb = "#663030",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Blue",
			//					ColorSquaresRgb = "#363656",
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Silver",
			//					ColorSquaresRgb = "#c5c5d5",
			//					DisplayOrder = 3,
			//				}
			//			}
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Shoes"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = false,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption =
			//				_specificationAttributeRepository.Table.Single(sa => sa.Name == "Color")
			//					.SpecificationAttributeOptions.Single(sao => sao.Name == "Grey")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = false,
			//			DisplayOrder = 2,
			//			SpecificationAttributeOption =
			//				_specificationAttributeRepository.Table.Single(sa => sa.Name == "Color")
			//					.SpecificationAttributeOptions.Single(sao => sao.Name == "Red")
			//		},
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = false,
			//			DisplayOrder = 3,
			//			SpecificationAttributeOption =
			//				_specificationAttributeRepository.Table.Single(sa => sa.Name == "Color")
			//					.SpecificationAttributeOptions.Single(sao => sao.Name == "Blue")
			//		},
			//	}
			//};
			//allProducts.Add(productAdidas);
			//productAdidas.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_adidas.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productAdidas.Name)),
			//	DisplayOrder = 1,
			//});
			//productAdidas.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_adidas_2.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productAdidas.Name)),
			//	DisplayOrder = 2,
			//});
			//productAdidas.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_adidas_3.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productAdidas.Name)),
			//	DisplayOrder = 3,
			//});


			//_productRepository.Insert(productAdidas);

			//productAdidas.ProductAttributeMappings.First(x => x.ProductAttribute.Name == "Color").ProductAttributeValues.First(x => x.Name == "Red").PictureId = productAdidas.ProductPictures.ElementAt(0).PictureId;
			//productAdidas.ProductAttributeMappings.First(x => x.ProductAttribute.Name == "Color").ProductAttributeValues.First(x => x.Name == "Blue").PictureId = productAdidas.ProductPictures.ElementAt(1).PictureId;
			//productAdidas.ProductAttributeMappings.First(x => x.ProductAttribute.Name == "Color").ProductAttributeValues.First(x => x.Name == "Silver").PictureId = productAdidas.ProductPictures.ElementAt(2).PictureId;
			//_productRepository.Update(productAdidas);




			//var productNikeZoom = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Nike SB Zoom Stefan Janoski \"Medium Mint\"",
			//	Sku = "NK_ZSJ_MM",
			//	ShortDescription = "Nike SB Zoom Stefan Janoski Dark Grey Medium Mint Teal ...",
			//	FullDescription = "The newly Nike SB Zoom Stefan Janoski gets hit with a \"Medium Mint\" accents that sits atop a Dark Grey suede. Expected to drop in October.",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "v-blue-juniors-cuffed-denim-short-with-rhinestones",
			//	AllowCustomerReviews = true,
			//	Price = 30M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Shoes"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "Nike"),
			//			DisplayOrder = 2,
			//		}
			//	},
			//	ProductSpecificationAttributes =
			//	{
			//		new ProductSpecificationAttribute
			//		{
			//			AllowFiltering = true,
			//			ShowOnProductPage = false,
			//			DisplayOrder = 1,
			//			SpecificationAttributeOption =
			//				_specificationAttributeRepository.Table.Single(sa => sa.Name == "Color")
			//					.SpecificationAttributeOptions.Single(sao => sao.Name == "Grey")
			//		}
			//	}
			//};

			//allProducts.Add(productNikeZoom);
			//productNikeZoom.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikeZoom.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productNikeZoom.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNikeZoom);


			//#endregion

			//#region Clothing

			//var productNikeTailwind = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Nike Tailwind Loose Short-Sleeve Running Shirt",
			//	Sku = "NK_TLS_RS",
			//	ShortDescription = "",
			//	FullDescription = "<p>Boost your adrenaline with the Nike® Women's Tailwind Running Shirt. The lightweight, slouchy fit is great for layering, and moisture-wicking fabrics keep you feeling at your best. This tee has a notched hem for an enhanced range of motion, while flat seams with reinforcement tape lessen discomfort and irritation over longer distances. Put your keys and card in the side zip pocket and take off in your Nike® running t-shirt.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "50s-rockabilly-polka-dot-top-jr-plus-size",
			//	AllowCustomerReviews = true,
			//	Published = true,
			//	Price = 15M,
			//	IsShipEnabled = true,
			//	Weight = 1,
			//	Length = 2,
			//	Width = 3,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Size"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Small",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "1X",
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "2X",
			//					DisplayOrder = 3,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "3X",
			//					DisplayOrder = 4,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "4X",
			//					DisplayOrder = 5,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "5X",
			//					DisplayOrder = 6,
			//				}
			//			}
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Clothing"),
			//			DisplayOrder = 1,
			//		}
			//	},
			//	ProductManufacturers =
			//	{
			//		new ProductManufacturer
			//		{
			//			Manufacturer = _manufacturerRepository.Table.Single(c => c.Name == "Nike"),
			//			DisplayOrder = 2,
			//		}
			//	}
			//};
			//allProducts.Add(productNikeTailwind);
			//productNikeTailwind.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NikeShirt.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productNikeTailwind.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNikeTailwind);




			//var productOversizedWomenTShirt = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Oversized Women T-Shirt",
			//	Sku = "WM_OVR_TS",
			//	ShortDescription = "",
			//	FullDescription = "<p>This oversized women t-Shirt needs minimum ironing. It is a great product at a great value!</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "arrow-mens-wrinkle-free-pinpoint-solid-long-sleeve",
			//	AllowCustomerReviews = true,
			//	Price = 24M,
			//	IsShipEnabled = true,
			//	Weight = 4,
			//	Length = 3,
			//	Width = 3,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	TierPrices =
			//	{
			//		new TierPrice
			//		{
			//			Quantity = 3,
			//			Price = 21
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 7,
			//			Price = 19
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 10,
			//			Price = 16
			//		}
			//	},
			//	HasTierPrices = true,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Clothing"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productOversizedWomenTShirt);
			//productOversizedWomenTShirt.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_WomenTShirt.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productOversizedWomenTShirt.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productOversizedWomenTShirt);




			//var productCustomTShirt = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Custom T-Shirt",
			//	Sku = "CS_TSHIRT",
			//	ShortDescription = "T-Shirt - Add Your Content",
			//	FullDescription = "<p>Comfort comes in all shapes and forms, yet this tee out does it all. Rising above the rest, our classic cotton crew provides the simple practicality you need to make it through the day. Tag-free, relaxed fit wears well under dress shirts or stands alone in laid-back style. Reinforced collar and lightweight feel give way to long-lasting shape and breathability. One less thing to worry about, rely on this tee to provide comfort and ease with every wear.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "custom-t-shirt",
			//	AllowCustomerReviews = true,
			//	Price = 15M,
			//	IsShipEnabled = true,
			//	Weight = 4,
			//	Length = 3,
			//	Width = 3,
			//	Height = 3,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Custom Text"),
			//			TextPrompt = "Enter your text:",
			//			AttributeControlType = AttributeControlType.TextBox,
			//			IsRequired = true,
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Clothing"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productCustomTShirt);
			//productCustomTShirt.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CustomTShirt.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productCustomTShirt.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productCustomTShirt);






			//var productLeviJeans = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Levi's 511 Jeans",
			//	Sku = "LV_511_JN",
			//	ShortDescription = "Levi's Faded Black 511 Jeans ",
			//	FullDescription = "<p>Between a skinny and straight fit, our 511&trade; slim fit jeans are cut close without being too restricting. Slim throughout the thigh and leg opening for a long and lean look.</p><ul><li>Slouch1y at top; sits below the waist</li><li>Slim through the leg, close at the thigh and straight to the ankle</li><li>Stretch for added comfort</li><li>Classic five-pocket styling</li><li>99% Cotton, 1% Spandex, 11.2 oz. - Imported</li></ul>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "levis-skinny-511-jeans",
			//	AllowCustomerReviews = true,
			//	Price = 43.5M,
			//	OldPrice = 55M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	TierPrices =
			//	{
			//		new TierPrice
			//		{
			//			Quantity = 3,
			//			Price = 40
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 6,
			//			Price = 38
			//		},
			//		new TierPrice
			//		{
			//			Quantity = 10,
			//			Price = 35
			//		}
			//	},
			//	HasTierPrices = true,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Clothing"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productLeviJeans);

			//productLeviJeans.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeviJeans_1.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productLeviJeans.Name)),
			//	DisplayOrder = 1,
			//});
			//productLeviJeans.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeviJeans_2.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productLeviJeans.Name)),
			//	DisplayOrder = 2,
			//});
			//_productRepository.Insert(productLeviJeans);


			//#endregion

			//#region Accessories


			//var productObeyHat = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Obey Propaganda Hat",
			//	Sku = "OB_HAT_PR",
			//	ShortDescription = "",
			//	FullDescription = "<p>Printed poplin 5 panel camp hat with debossed leather patch and web closure</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "indiana-jones-shapeable-wool-hat",
			//	AllowCustomerReviews = true,
			//	Price = 30M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductAttributeMappings =
			//	{
			//		new ProductAttributeMapping
			//		{
			//			ProductAttribute = _productAttributeRepository.Table.Single(x => x.Name == "Size"),
			//			AttributeControlType = AttributeControlType.DropdownList,
			//			IsRequired = true,
			//			ProductAttributeValues =
			//			{
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Small",
			//					DisplayOrder = 1,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Medium",
			//					DisplayOrder = 2,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "Large",
			//					DisplayOrder = 3,
			//				},
			//				new ProductAttributeValue
			//				{
			//					AttributeValueType = AttributeValueType.Simple,
			//					Name = "X-Large",
			//					DisplayOrder = 4,
			//				}
			//			}
			//		}
			//	},
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productObeyHat);
			//productObeyHat.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_hat.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productObeyHat.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productObeyHat);







			//var productBelt = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Reversible Horseferry Check Belt",
			//	Sku = "RH_CHK_BL",
			//	ShortDescription = "Reversible belt in Horseferry check with smooth leather trim",
			//	FullDescription = "<p>Reversible belt in Horseferry check with smooth leather trim</p><p>Leather lining, polished metal buckle</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "nike-golf-casual-belt",
			//	AllowCustomerReviews = true,
			//	Price = 45M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	ProductAvailabilityRangeId = productAvailabilityRange.Id,
			//	StockQuantity = 0,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productBelt);
			//productBelt.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Belt.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productBelt.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productBelt);






			//var productSunglasses = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Ray Ban Aviator Sunglasses",
			//	Sku = "RB_AVR_SG",
			//	ShortDescription = "Aviator sunglasses are one of the first widely popularized styles of modern day sunwear.",
			//	FullDescription = "<p>Since 1937, Ray-Ban can genuinely claim the title as the world's leading sunglasses and optical eyewear brand. Combining the best of fashion and sports performance, the Ray-Ban line of Sunglasses delivers a truly classic style that will have you looking great today and for years to come.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "ray-ban-aviator-sunglasses-rb-3025",
			//	AllowCustomerReviews = true,
			//	Price = 25M,
			//	IsShipEnabled = true,
			//	Weight = 7,
			//	Length = 7,
			//	Width = 7,
			//	Height = 7,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Apparel").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productSunglasses);
			//productSunglasses.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Sunglasses.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productSunglasses.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productSunglasses);

			//#endregion

			//#region Digital Downloads


			//var downloadNightVision1 = new Download
			//{
			//	DownloadGuid = Guid.NewGuid(),
			//	ContentType = MimeTypes.ApplicationXZipCo,
			//	DownloadBinary = File.ReadAllBytes(sampleDownloadsPath + "product_NightVision_1.zip"),
			//	Extension = ".zip",
			//	Filename = "Night_Vision_1",
			//	IsNew = true,
			//};
			//downloadService.InsertDownload(downloadNightVision1);
			//var downloadNightVision2 = new Download
			//{
			//	DownloadGuid = Guid.NewGuid(),
			//	ContentType = MimeTypes.TextPlain,
			//	DownloadBinary = File.ReadAllBytes(sampleDownloadsPath + "product_NightVision_2.txt"),
			//	Extension = ".txt",
			//	Filename = "Night_Vision_1",
			//	IsNew = true,
			//};
			//downloadService.InsertDownload(downloadNightVision2);
			//var productNightVision = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Night Visions",
			//	Sku = "NIGHT_VSN",
			//	ShortDescription = "Night Visions is the debut studio album by American rock band Imagine Dragons.",
			//	FullDescription = "<p>Original Release Date: September 4, 2012</p><p>Release Date: September 4, 2012</p><p>Genre - Alternative rock, indie rock, electronic rock</p><p>Label - Interscope/KIDinaKORNER</p><p>Copyright: (C) 2011 Interscope Records</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "poker-face",
			//	AllowCustomerReviews = true,
			//	Price = 2.8M,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Downloadable Products").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	IsDownload = true,
			//	DownloadId = downloadNightVision1.Id,
			//	DownloadActivationType = DownloadActivationType.WhenOrderIsPaid,
			//	UnlimitedDownloads = true,
			//	HasUserAgreement = false,
			//	HasSampleDownload = true,
			//	SampleDownloadId = downloadNightVision2.Id,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Digital downloads"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productNightVision);
			//productNightVision.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_NightVisions.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productNightVision.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productNightVision);





			//var downloadIfYouWait1 = new Download
			//{
			//	DownloadGuid = Guid.NewGuid(),
			//	ContentType = MimeTypes.ApplicationXZipCo,
			//	DownloadBinary = File.ReadAllBytes(sampleDownloadsPath + "product_IfYouWait_1.zip"),
			//	Extension = ".zip",
			//	Filename = "If_You_Wait_1",
			//	IsNew = true,
			//};
			//downloadService.InsertDownload(downloadIfYouWait1);
			//var downloadIfYouWait2 = new Download
			//{
			//	DownloadGuid = Guid.NewGuid(),
			//	ContentType = MimeTypes.TextPlain,
			//	DownloadBinary = File.ReadAllBytes(sampleDownloadsPath + "product_IfYouWait_2.txt"),
			//	Extension = ".txt",
			//	Filename = "If_You_Wait_1",
			//	IsNew = true,
			//};
			//downloadService.InsertDownload(downloadIfYouWait2);
			//var productIfYouWait = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "If You Wait (donation)",
			//	Sku = "IF_YOU_WT",
			//	ShortDescription = "If You Wait is the debut studio album by English indie pop band London Grammar",
			//	FullDescription = "<p>Original Release Date: September 6, 2013</p><p>Genre - Electronica, dream pop downtempo, pop</p><p>Label - Metal & Dust/Ministry of Sound</p><p>Producer - Tim Bran, Roy Kerr London, Grammar</p><p>Length - 43:22</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "single-ladies-put-a-ring-on-it",
			//	CustomerEntersPrice = true,
			//	MinimumCustomerEnteredPrice = 0.5M,
			//	MaximumCustomerEnteredPrice = 100M,
			//	AllowCustomerReviews = true,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Downloadable Products").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	IsDownload = true,
			//	DownloadId = downloadIfYouWait1.Id,
			//	DownloadActivationType = DownloadActivationType.WhenOrderIsPaid,
			//	UnlimitedDownloads = true,
			//	HasUserAgreement = false,
			//	HasSampleDownload = true,
			//	SampleDownloadId = downloadIfYouWait2.Id,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Digital downloads"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productIfYouWait);

			//productIfYouWait.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_IfYouWait.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productIfYouWait.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productIfYouWait);





			//var downloadScienceAndFaith = new Download
			//{
			//	DownloadGuid = Guid.NewGuid(),
			//	ContentType = MimeTypes.ApplicationXZipCo,
			//	DownloadBinary = File.ReadAllBytes(sampleDownloadsPath + "product_ScienceAndFaith_1.zip"),
			//	Extension = ".zip",
			//	Filename = "Science_And_Faith",
			//	IsNew = true,
			//};
			//downloadService.InsertDownload(downloadScienceAndFaith);
			//var productScienceAndFaith = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Science & Faith",
			//	Sku = "SCI_FAITH",
			//	ShortDescription = "Science & Faith is the second studio album by Irish pop rock band The Script.",
			//	FullDescription = "<p># Original Release Date: September 10, 2010<br /># Label: RCA, Epic/Phonogenic(America)<br /># Copyright: 2010 RCA Records.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "the-battle-of-los-angeles",
			//	AllowCustomerReviews = true,
			//	CustomerEntersPrice = true,
			//	MinimumCustomerEnteredPrice = 0.5M,
			//	MaximumCustomerEnteredPrice = 1000M,
			//	Price = decimal.Zero,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Downloadable Products").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	IsDownload = true,
			//	DownloadId = downloadScienceAndFaith.Id,
			//	DownloadActivationType = DownloadActivationType.WhenOrderIsPaid,
			//	UnlimitedDownloads = true,
			//	HasUserAgreement = false,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Digital downloads"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productScienceAndFaith);
			//productScienceAndFaith.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_ScienceAndFaith.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productScienceAndFaith.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productScienceAndFaith);



			//#endregion

			//#region Books

			//var productFahrenheit = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Fahrenheit 451 by Ray Bradbury",
			//	Sku = "FR_451_RB",
			//	ShortDescription = "Fahrenheit 451 is a dystopian novel by Ray Bradbury published in 1953. It is regarded as one of his best works.",
			//	FullDescription = "<p>The novel presents a future American society where books are outlawed and firemen burn any that are found. The title refers to the temperature that Bradbury understood to be the autoignition point of paper.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "best-grilling-recipes",
			//	AllowCustomerReviews = true,
			//	Price = 27M,
			//	OldPrice = 30M,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Books").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productFahrenheit);
			//productFahrenheit.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Fahrenheit451.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productFahrenheit.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productFahrenheit);



			//var productFirstPrizePies = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "First Prize Pies",
			//	Sku = "FIRST_PRP",
			//	ShortDescription = "Allison Kave made pies as a hobby, until one day her boyfriend convinced her to enter a Brooklyn pie-making contest. She won. In fact, her pies were such a hit that she turned pro.",
			//	FullDescription = "<p>First Prize Pies, a boutique, made-to-order pie business that originated on New York's Lower East Side, has become synonymous with tempting and unusual confections. For the home baker who is passionate about seasonal ingredients and loves a creative approach to recipes, First Prize Pies serves up 52 weeks of seasonal and eclectic pastries in an interesting pie-a-week format. Clear instructions, technical tips and creative encouragement guide novice bakers as well as pie mavens. With its nostalgia-evoking photos of homemade pies fresh out of the oven, First Prize Pies will be as giftable as it is practical.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "eatingwell-in-season",
			//	AllowCustomerReviews = true,
			//	Price = 51M,
			//	OldPrice = 67M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Books").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productFirstPrizePies);
			//productFirstPrizePies.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_FirstPrizePies.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productFirstPrizePies.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productFirstPrizePies);







			//var productPrideAndPrejudice = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Pride and Prejudice",
			//	Sku = "PRIDE_PRJ",
			//	ShortDescription = "Pride and Prejudice is a novel of manners by Jane Austen, first published in 1813.",
			//	FullDescription = "<p>Set in England in the early 19th century, Pride and Prejudice tells the story of Mr and Mrs Bennet's five unmarried daughters after the rich and eligible Mr Bingley and his status-conscious friend, Mr Darcy, have moved into their neighbourhood. While Bingley takes an immediate liking to the eldest Bennet daughter, Jane, Darcy has difficulty adapting to local society and repeatedly clashes with the second-eldest Bennet daughter, Elizabeth.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "the-best-skillet-recipes",
			//	AllowCustomerReviews = true,
			//	Price = 24M,
			//	OldPrice = 35M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Books").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productPrideAndPrejudice);
			//productPrideAndPrejudice.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_PrideAndPrejudice.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(productPrideAndPrejudice.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productPrideAndPrejudice);



			//#endregion

			//#region Jewelry



			//var productElegantGemstoneNecklace = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Elegant Gemstone Necklace (rental)",
			//	Sku = "EG_GEM_NL",
			//	ShortDescription = "Classic and elegant gemstone necklace now available in our store",
			//	FullDescription = "<p>For those who like jewelry, creating their ownelegant jewelry from gemstone beads provides an economical way to incorporate genuine gemstones into your jewelry wardrobe. Manufacturers create beads from all kinds of precious gemstones and semi-precious gemstones, which are available in bead shops, craft stores, and online marketplaces.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "diamond-pave-earrings",
			//	AllowCustomerReviews = true,
			//	IsRental = true,
			//	RentalPriceLength = 1,
			//	RentalPricePeriod = RentalPricePeriod.Days,
			//	Price = 30M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Jewelry").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productElegantGemstoneNecklace);
			//productElegantGemstoneNecklace.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_GemstoneNecklaces.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productElegantGemstoneNecklace.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productElegantGemstoneNecklace);





			//var productFlowerGirlBracelet = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Flower Girl Bracelet",
			//	Sku = "FL_GIRL_B",
			//	ShortDescription = "Personalised Flower Braceled",
			//	FullDescription = "<p>This is a great gift for your flower girl to wear on your wedding day. A delicate bracelet that is made with silver plated soldered cable chain, gives this bracelet a dainty look for young wrist. A Swarovski heart, shown in Rose, hangs off a silver plated flower. Hanging alongside the heart is a silver plated heart charm with Flower Girl engraved on both sides. This is a great style for the younger flower girl.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "diamond-tennis-bracelet",
			//	AllowCustomerReviews = true,
			//	Price = 360M,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Jewelry").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productFlowerGirlBracelet);
			//productFlowerGirlBracelet.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_FlowerBracelet.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productFlowerGirlBracelet.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productFlowerGirlBracelet);








			//var productEngagementRing = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "Vintage Style Engagement Ring",
			//	Sku = "VS_ENG_RN",
			//	ShortDescription = "1.24 Carat (ctw) in 14K White Gold (Certified)",
			//	FullDescription = "<p>Dazzle her with this gleaming 14 karat white gold vintage proposal. A ravishing collection of 11 decadent diamonds come together to invigorate a superbly ornate gold shank. Total diamond weight on this antique style engagement ring equals 1 1/4 carat (ctw). Item includes diamond certificate.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "vintage-style-three-stone-diamond-engagement-ring",
			//	AllowCustomerReviews = true,
			//	Price = 2100M,
			//	IsShipEnabled = true,
			//	Weight = 2,
			//	Length = 2,
			//	Width = 2,
			//	Height = 2,
			//	TaxCategoryId = _taxCategoryRepository.Table.Single(tc => tc.Name == "Jewelry").Id,
			//	ManageInventoryMethod = ManageInventoryMethod.ManageStock,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	DisplayStockAvailability = true,
			//	LowStockActivity = LowStockActivity.DisableBuyButton,
			//	BackorderMode = BackorderMode.NoBackorders,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
			//			DisplayOrder = 1,
			//		}
			//	}
			//};
			//allProducts.Add(productEngagementRing);
			//productEngagementRing.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_EngagementRing_1.jpg"), MimeTypes.ImagePJpeg, pictureService.GetPictureSeName(productEngagementRing.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(productEngagementRing);



			//#endregion

			//#region Gift Cards


			//var product25GiftCard = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "$25 Virtual Gift Card",
			//	Sku = "VG_CR_025",
			//	ShortDescription = "$25 Gift Card. Gift Cards must be redeemed through our site Web site toward the purchase of eligible products.",
			//	FullDescription = "<p>Gift Cards must be redeemed through our site Web site toward the purchase of eligible products. Purchases are deducted from the GiftCard balance. Any unused balance will be placed in the recipient's GiftCard account when redeemed. If an order exceeds the amount of the GiftCard, the balance must be paid with a credit card or other available payment method.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "25-virtual-gift-card",
			//	AllowCustomerReviews = true,
			//	Price = 25M,
			//	IsGiftCard = true,
			//	GiftCardType = GiftCardType.Virtual,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	Published = true,
			//	ShowOnHomePage = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Gift Cards"),
			//			DisplayOrder = 2,
			//		}
			//	}
			//};
			//allProducts.Add(product25GiftCard);
			//product25GiftCard.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_25giftcart.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(product25GiftCard.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(product25GiftCard);





			//var product50GiftCard = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "$50 Physical Gift Card",
			//	Sku = "PG_CR_050",
			//	ShortDescription = "$50 Gift Card. Gift Cards must be redeemed through our site Web site toward the purchase of eligible products.",
			//	FullDescription = "<p>Gift Cards must be redeemed through our site Web site toward the purchase of eligible products. Purchases are deducted from the GiftCard balance. Any unused balance will be placed in the recipient's GiftCard account when redeemed. If an order exceeds the amount of the GiftCard, the balance must be paid with a credit card or other available payment method.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "50-physical-gift-card",
			//	AllowCustomerReviews = true,
			//	Price = 50M,
			//	IsGiftCard = true,
			//	GiftCardType = GiftCardType.Physical,
			//	IsShipEnabled = true,
			//	IsFreeShipping = true,
			//	DeliveryDateId = deliveryDate.Id,
			//	Weight = 1,
			//	Length = 1,
			//	Width = 1,
			//	Height = 1,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	Published = true,
			//	MarkAsNew = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Gift Cards"),
			//			DisplayOrder = 3,
			//		}
			//	}
			//};
			//allProducts.Add(product50GiftCard);
			//product50GiftCard.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_50giftcart.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(product50GiftCard.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(product50GiftCard);





			//var product100GiftCard = new Product
			//{
			//	ProductType = ProductType.SimpleProduct,
			//	VisibleIndividually = true,
			//	Name = "$100 Physical Gift Card",
			//	Sku = "PG_CR_100",
			//	ShortDescription = "$100 Gift Card. Gift Cards must be redeemed through our site Web site toward the purchase of eligible products.",
			//	FullDescription = "<p>Gift Cards must be redeemed through our site Web site toward the purchase of eligible products. Purchases are deducted from the GiftCard balance. Any unused balance will be placed in the recipient's GiftCard account when redeemed. If an order exceeds the amount of the GiftCard, the balance must be paid with a credit card or other available payment method.</p>",
			//	ProductTemplateId = productTemplateSimple.Id,
			//	//SeName = "100-physical-gift-card",
			//	AllowCustomerReviews = true,
			//	Price = 100M,
			//	IsGiftCard = true,
			//	GiftCardType = GiftCardType.Physical,
			//	IsShipEnabled = true,
			//	DeliveryDateId = deliveryDate.Id,
			//	Weight = 1,
			//	Length = 1,
			//	Width = 1,
			//	Height = 1,
			//	ManageInventoryMethod = ManageInventoryMethod.DontManageStock,
			//	OrderMinimumQuantity = 1,
			//	OrderMaximumQuantity = 10000,
			//	StockQuantity = 10000,
			//	NotifyAdminForQuantityBelow = 1,
			//	AllowBackInStockSubscriptions = false,
			//	Published = true,
			//	CreatedOnUtc = DateTime.UtcNow,
			//	UpdatedOnUtc = DateTime.UtcNow,
			//	ProductCategories =
			//	{
			//		new ProductCategory
			//		{
			//			Category = _categoryRepository.Table.Single(c => c.Name == "Gift Cards"),
			//			DisplayOrder = 4,
			//		}
			//	}
			//};
			//allProducts.Add(product100GiftCard);
			//product100GiftCard.ProductPictures.Add(new ProductPicture
			//{
			//	Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_100giftcart.jpeg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(product100GiftCard.Name)),
			//	DisplayOrder = 1,
			//});
			//_productRepository.Insert(product100GiftCard);

			//#endregion



			////search engine names
			//foreach (var product in allProducts)
			//{
			//	_urlRecordRepository.Insert(new UrlRecord
			//	{
			//		EntityId = product.Id,
			//		EntityName = "Product",
			//		LanguageId = 0,
			//		IsActive = true,
			//		Slug = product.ValidateSeName("", product.Name, true)
			//	});
			//}


			//#region Related Products

			////related products
			//var relatedProducts = new List<RelatedProduct>
			//{
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFlowerGirlBracelet.Id,
			//		 ProductId2 = productEngagementRing.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFlowerGirlBracelet.Id,
			//		 ProductId2 = productElegantGemstoneNecklace.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productEngagementRing.Id,
			//		 ProductId2 = productFlowerGirlBracelet.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productEngagementRing.Id,
			//		 ProductId2 = productElegantGemstoneNecklace.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productElegantGemstoneNecklace.Id,
			//		 ProductId2 = productFlowerGirlBracelet.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productElegantGemstoneNecklace.Id,
			//		 ProductId2 = productEngagementRing.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productIfYouWait.Id,
			//		 ProductId2 = productNightVision.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productIfYouWait.Id,
			//		 ProductId2 = productScienceAndFaith.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNightVision.Id,
			//		 ProductId2 = productIfYouWait.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNightVision.Id,
			//		 ProductId2 = productScienceAndFaith.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productPrideAndPrejudice.Id,
			//		 ProductId2 = productFirstPrizePies.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productPrideAndPrejudice.Id,
			//		 ProductId2 = productFahrenheit.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFirstPrizePies.Id,
			//		 ProductId2 = productPrideAndPrejudice.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFirstPrizePies.Id,
			//		 ProductId2 = productFahrenheit.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFahrenheit.Id,
			//		 ProductId2 = productFirstPrizePies.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productFahrenheit.Id,
			//		 ProductId2 = productPrideAndPrejudice.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAsusN551JK.Id,
			//		 ProductId2 = productLenovoThinkpad.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAsusN551JK.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAsusN551JK.Id,
			//		 ProductId2 = productSamsungSeries.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAsusN551JK.Id,
			//		 ProductId2 = productHpSpectre.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoThinkpad.Id,
			//		 ProductId2 = productAsusN551JK.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoThinkpad.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoThinkpad.Id,
			//		 ProductId2 = productSamsungSeries.Id,
			//	},
			//	 new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoThinkpad.Id,
			//		 ProductId2 = productHpEnvy.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAppleMacBookPro.Id,
			//		 ProductId2 = productLenovoThinkpad.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAppleMacBookPro.Id,
			//		 ProductId2 = productSamsungSeries.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAppleMacBookPro.Id,
			//		 ProductId2 = productAsusN551JK.Id,
			//	},
			//	 new RelatedProduct
			//	{
			//		 ProductId1 = productAppleMacBookPro.Id,
			//		 ProductId2 = productHpSpectre.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpSpectre.Id,
			//		 ProductId2 = productLenovoThinkpad.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpSpectre.Id,
			//		 ProductId2 = productSamsungSeries.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpSpectre.Id,
			//		 ProductId2 = productAsusN551JK.Id,
			//	},
			//	 new RelatedProduct
			//	{
			//		 ProductId1 = productHpSpectre.Id,
			//		 ProductId2 = productHpEnvy.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpEnvy.Id,
			//		 ProductId2 = productAsusN551JK.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpEnvy.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpEnvy.Id,
			//		 ProductId2 = productHpSpectre.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHpEnvy.Id,
			//		 ProductId2 = productSamsungSeries.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productSamsungSeries.Id,
			//		 ProductId2 = productAsusN551JK.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productSamsungSeries.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productSamsungSeries.Id,
			//		 ProductId2 = productHpEnvy.Id,
			//	},
			//	 new RelatedProduct
			//	{
			//		 ProductId1 = productSamsungSeries.Id,
			//		 ProductId2 = productHpSpectre.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeica.Id,
			//		 ProductId2 = productHtcOneMini.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeica.Id,
			//		 ProductId2 = productNikonD5500DSLR.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeica.Id,
			//		 ProductId2 = productAppleICam.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeica.Id,
			//		 ProductId2 = productNokiaLumia.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOne.Id,
			//		 ProductId2 = productHtcOneMini.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOne.Id,
			//		 ProductId2 = productNokiaLumia.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOne.Id,
			//		 ProductId2 = productBeatsPill.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOne.Id,
			//		 ProductId2 = productPortableSoundSpeakers.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOneMini.Id,
			//		 ProductId2 = productHtcOne.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOneMini.Id,
			//		 ProductId2 = productNokiaLumia.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOneMini.Id,
			//		 ProductId2 = productBeatsPill.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productHtcOneMini.Id,
			//		 ProductId2 = productPortableSoundSpeakers.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNokiaLumia.Id,
			//		 ProductId2 = productHtcOne.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNokiaLumia.Id,
			//		 ProductId2 = productHtcOneMini.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNokiaLumia.Id,
			//		 ProductId2 = productBeatsPill.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productNokiaLumia.Id,
			//		 ProductId2 = productPortableSoundSpeakers.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAdidas.Id,
			//		 ProductId2 = productLeviJeans.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAdidas.Id,
			//		 ProductId2 = productNikeFloral.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAdidas.Id,
			//		 ProductId2 = productNikeZoom.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productAdidas.Id,
			//		 ProductId2 = productNikeTailwind.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeviJeans.Id,
			//		 ProductId2 = productAdidas.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeviJeans.Id,
			//		 ProductId2 = productNikeFloral.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeviJeans.Id,
			//		 ProductId2 = productNikeZoom.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLeviJeans.Id,
			//		 ProductId2 = productNikeTailwind.Id,
			//	},

			//	new RelatedProduct
			//	{
			//		 ProductId1 = productCustomTShirt.Id,
			//		 ProductId2 = productLeviJeans.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productCustomTShirt.Id,
			//		 ProductId2 = productNikeTailwind.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productCustomTShirt.Id,
			//		 ProductId2 = productOversizedWomenTShirt.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productCustomTShirt.Id,
			//		 ProductId2 = productObeyHat.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productDigitalStorm.Id,
			//		 ProductId2 = productBuildComputer.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productDigitalStorm.Id,
			//		 ProductId2 = productLenovoIdeaCentre.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productDigitalStorm.Id,
			//		 ProductId2 = productLenovoThinkpad.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productDigitalStorm.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},


			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoIdeaCentre.Id,
			//		 ProductId2 = productBuildComputer.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoIdeaCentre.Id,
			//		 ProductId2 = productDigitalStorm.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoIdeaCentre.Id,
			//		 ProductId2 = productLenovoThinkpad.Id,
			//	},
			//	new RelatedProduct
			//	{
			//		 ProductId1 = productLenovoIdeaCentre.Id,
			//		 ProductId2 = productAppleMacBookPro.Id,
			//	},
			//};
			//_relatedProductRepository.Insert(relatedProducts);

			//#endregion

			//#region Product Tags

			////product tags
			//AddProductTag(product25GiftCard, "nice");
			//AddProductTag(product25GiftCard, "gift");
			//AddProductTag(productNikeTailwind, "cool");
			//AddProductTag(productNikeTailwind, "apparel");
			//AddProductTag(productNikeTailwind, "shirt");
			//AddProductTag(productBeatsPill, "computer");
			//AddProductTag(productBeatsPill, "cool");
			//AddProductTag(productNikeFloral, "cool");
			//AddProductTag(productNikeFloral, "shoes");
			//AddProductTag(productNikeFloral, "apparel");
			//AddProductTag(productAdobePhotoshop, "computer");
			//AddProductTag(productAdobePhotoshop, "awesome");
			//AddProductTag(productUniversalTabletCover, "computer");
			//AddProductTag(productUniversalTabletCover, "cool");
			//AddProductTag(productOversizedWomenTShirt, "cool");
			//AddProductTag(productOversizedWomenTShirt, "apparel");
			//AddProductTag(productOversizedWomenTShirt, "shirt");
			//AddProductTag(productAppleMacBookPro, "compact");
			//AddProductTag(productAppleMacBookPro, "awesome");
			//AddProductTag(productAppleMacBookPro, "computer");
			//AddProductTag(productAsusN551JK, "compact");
			//AddProductTag(productAsusN551JK, "awesome");
			//AddProductTag(productAsusN551JK, "computer");
			//AddProductTag(productFahrenheit, "awesome");
			//AddProductTag(productFahrenheit, "book");
			//AddProductTag(productFahrenheit, "nice");
			//AddProductTag(productHtcOne, "cell");
			//AddProductTag(productHtcOne, "compact");
			//AddProductTag(productHtcOne, "awesome");
			//AddProductTag(productBuildComputer, "awesome");
			//AddProductTag(productBuildComputer, "computer");
			//AddProductTag(productNikonD5500DSLR, "cool");
			//AddProductTag(productNikonD5500DSLR, "camera");
			//AddProductTag(productLeica, "camera");
			//AddProductTag(productLeica, "cool");
			//AddProductTag(productDigitalStorm, "cool");
			//AddProductTag(productDigitalStorm, "computer");
			//AddProductTag(productWindows8Pro, "awesome");
			//AddProductTag(productWindows8Pro, "computer");
			//AddProductTag(productCustomTShirt, "cool");
			//AddProductTag(productCustomTShirt, "shirt");
			//AddProductTag(productCustomTShirt, "apparel");
			//AddProductTag(productElegantGemstoneNecklace, "jewelry");
			//AddProductTag(productElegantGemstoneNecklace, "awesome");
			//AddProductTag(productFlowerGirlBracelet, "awesome");
			//AddProductTag(productFlowerGirlBracelet, "jewelry");
			//AddProductTag(productFirstPrizePies, "book");
			//AddProductTag(productAdidas, "cool");
			//AddProductTag(productAdidas, "shoes");
			//AddProductTag(productAdidas, "apparel");
			//AddProductTag(productLenovoIdeaCentre, "awesome");
			//AddProductTag(productLenovoIdeaCentre, "computer");
			//AddProductTag(productSamsungSeries, "nice");
			//AddProductTag(productSamsungSeries, "computer");
			//AddProductTag(productSamsungSeries, "compact");
			//AddProductTag(productHpSpectre, "nice");
			//AddProductTag(productHpSpectre, "computer");
			//AddProductTag(productHpEnvy, "computer");
			//AddProductTag(productHpEnvy, "cool");
			//AddProductTag(productHpEnvy, "compact");
			//AddProductTag(productObeyHat, "apparel");
			//AddProductTag(productObeyHat, "cool");
			//AddProductTag(productLeviJeans, "cool");
			//AddProductTag(productLeviJeans, "jeans");
			//AddProductTag(productLeviJeans, "apparel");
			//AddProductTag(productSoundForge, "game");
			//AddProductTag(productSoundForge, "computer");
			//AddProductTag(productSoundForge, "cool");
			//AddProductTag(productNightVision, "awesome");
			//AddProductTag(productNightVision, "digital");
			//AddProductTag(productSunglasses, "apparel");
			//AddProductTag(productSunglasses, "cool");
			//AddProductTag(productHtcOneMini, "awesome");
			//AddProductTag(productHtcOneMini, "compact");
			//AddProductTag(productHtcOneMini, "cell");
			//AddProductTag(productIfYouWait, "digital");
			//AddProductTag(productIfYouWait, "awesome");
			//AddProductTag(productNokiaLumia, "awesome");
			//AddProductTag(productNokiaLumia, "cool");
			//AddProductTag(productNokiaLumia, "camera");
			//AddProductTag(productScienceAndFaith, "digital");
			//AddProductTag(productScienceAndFaith, "awesome");
			//AddProductTag(productPrideAndPrejudice, "book");
			//AddProductTag(productLenovoThinkpad, "awesome");
			//AddProductTag(productLenovoThinkpad, "computer");
			//AddProductTag(productLenovoThinkpad, "compact");
			//AddProductTag(productNikeZoom, "jeans");
			//AddProductTag(productNikeZoom, "cool");
			//AddProductTag(productNikeZoom, "apparel");
			//AddProductTag(productEngagementRing, "jewelry");
			//AddProductTag(productEngagementRing, "awesome");


			//#endregion

			//#region  Reviews

			////reviews
			//var random = new Random();
			//foreach (var product in allProducts)
			//{
			//	if (product.ProductType != ProductType.SimpleProduct)
			//		continue;

			//	//only 3 of 4 products will have reviews
			//	if (random.Next(4) == 3)
			//		continue;

			//	//rating from 4 to 5
			//	var rating = random.Next(4, 6);
			//	product.ProductReviews.Add(new ProductReview
			//	{
			//		CustomerId = defaultCustomer.Id,
			//		ProductId = product.Id,
			//		StoreId = defaultStore.Id,
			//		IsApproved = true,
			//		Title = "Some sample review",
			//		ReviewText = string.Format("This sample review is for the {0}. I've been waiting for this product to be available. It is priced just right.", product.Name),
			//		//random (4 or 5)
			//		Rating = rating,
			//		HelpfulYesTotal = 0,
			//		HelpfulNoTotal = 0,
			//		CreatedOnUtc = DateTime.UtcNow
			//	});
			//	product.ApprovedRatingSum = rating;
			//	product.ApprovedTotalReviews = product.ProductReviews.Count;

			//}
			//_productRepository.Update(allProducts);

			//#endregion

			//#region Stock quantity history

			//foreach (var product in allProducts)
			//{
			//	if (product.StockQuantity > 0)
			//		_stockQuantityHistoryRepository.Insert(new StockQuantityHistory
			//		{
			//			ProductId = product.Id,
			//			WarehouseId = product.WarehouseId > 0 ? (int?)product.WarehouseId : null,
			//			QuantityAdjustment = product.StockQuantity,
			//			StockQuantity = product.StockQuantity,
			//			Message = "The stock quantity has been edited",
			//			CreatedOnUtc = DateTime.UtcNow
			//		});
			//}

			//#endregion
		}

		protected virtual void InstallForums()
		{
			var forumGroup = new ForumGroup
			{
				Name = "General",
				DisplayOrder = 5,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};

			_forumGroupRepository.Insert(forumGroup);

			var newProductsForum = new Forum
			{
				ForumGroup = forumGroup,
				Name = "New Products",
				Description = "Discuss new products and industry trends",
				NumTopics = 0,
				NumPosts = 0,
				LastPostCustomerId = 0,
				LastPostTime = null,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
			_forumRepository.Insert(newProductsForum);

			var mobileDevicesForum = new Forum
			{
				ForumGroup = forumGroup,
				Name = "Mobile Devices Forum",
				Description = "Discuss the mobile phone market",
				NumTopics = 0,
				NumPosts = 0,
				LastPostCustomerId = 0,
				LastPostTime = null,
				DisplayOrder = 10,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
			_forumRepository.Insert(mobileDevicesForum);

			var packagingShippingForum = new Forum
			{
				ForumGroup = forumGroup,
				Name = "Packaging & Shipping",
				Description = "Discuss packaging & shipping",
				NumTopics = 0,
				NumPosts = 0,
				LastPostTime = null,
				DisplayOrder = 20,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
			_forumRepository.Insert(packagingShippingForum);
		}

		protected virtual void InstallDiscounts()
		{
			var discounts = new List<Discount>
								{
									new Discount
										{
											Name = "Sample discount with coupon code",
											DiscountType = DiscountType.AssignedToSkus,
											DiscountLimitation = DiscountLimitationType.Unlimited,
											UsePercentage = false,
											DiscountAmount = 10,
											RequiresCouponCode = true,
											CouponCode = "123",
										},
									new Discount
										{
											Name = "'20% order total' discount",
											DiscountType = DiscountType.AssignedToOrderTotal,
											DiscountLimitation = DiscountLimitationType.Unlimited,
											UsePercentage = true,
											DiscountPercentage = 20,
											StartDateUtc = new DateTime(2010,1,1),
											EndDateUtc = new DateTime(2020,1,1),
											RequiresCouponCode = true,
											CouponCode = "456",
										},
								};
			_discountRepository.Insert(discounts);
		}

		protected virtual void InstallBlogPosts(string defaultUserEmail)
		{
			var defaultLanguage = _languageRepository.Table.FirstOrDefault();

			var blogPosts = new List<BlogPost>
								{
									new BlogPost
										{
											 AllowComments = true,
											 Language = defaultLanguage,
											 Title = "How a blog can help your growing e-Commerce business",
											 BodyOverview = "<p>When you start an online business, your main aim is to sell the products, right? As a business owner, you want to showcase your store to more audience. So, you decide to go on social media, why? Because everyone is doing it, then why shouldn&rsquo;t you? It is tempting as everyone is aware of the hype that it is the best way to market your brand.</p><p>Do you know having a blog for your online store can be very helpful? Many businesses do not understand the importance of having a blog because they don&rsquo;t have time to post quality content.</p><p>Today, we will talk about how a blog can play an important role for the growth of your e-Commerce business. Later, we will also discuss some tips that will be helpful to you for writing business related blog posts.</p>",
											 Body = "<p>When you start an online business, your main aim is to sell the products, right? As a business owner, you want to showcase your store to more audience. So, you decide to go on social media, why? Because everyone is doing it, then why shouldn&rsquo;t you? It is tempting as everyone is aware of the hype that it is the best way to market your brand.</p><p>Do you know having a blog for your online store can be very helpful? Many businesses do not understand the importance of having a blog because they don&rsquo;t have time to post quality content.</p><p>Today, we will talk about how a blog can play an important role for the growth of your e-Commerce business. Later, we will also discuss some tips that will be helpful to you for writing business related blog posts.</p><h3>1) Blog is useful in educating your customers</h3><p>Blogging is one of the best way by which you can educate your customers about your products/services that you offer. This helps you as a business owner to bring more value to your brand. When you provide useful information to the customers about your products, they are more likely to buy products from you. You can use your blog for providing tutorials in regard to the use of your products.</p><p><strong>For example:</strong> If you have an online store that offers computer parts. You can write tutorials about how to build a computer or how to make your computer&rsquo;s performance better. While talking about these things, you can mention products in the tutorials and provide link to your products within the blog post from your website. Your potential customers might get different ideas of using your product and will likely to buy products from your online store.</p><h3>2) Blog helps your business in Search Engine Optimization (SEO)</h3><p>Blog posts create more internal links to your website which helps a lot in SEO. Blog is a great way to have quality content on your website related to your products/services which is indexed by all major search engines like Google, Bing and Yahoo. The more original content you write in your blog post, the better ranking you will get in search engines. SEO is an on-going process and posting blog posts regularly keeps your site active all the time which is beneficial when it comes to search engine optimization.</p><p><strong>For example:</strong> Let&rsquo;s say you sell &ldquo;Sony Television Model XYZ&rdquo; and you regularly publish blog posts about your product. Now, whenever someone searches for &ldquo;Sony Television Model XYZ&rdquo;, Google will crawl on your website knowing that you have something to do with this particular product. Hence, your website will show up on the search result page whenever this item is being searched.</p><h3>3) Blog helps in boosting your sales by convincing the potential customers to buy</h3><p>If you own an online business, there are so many ways you can share different stories with your audience in regard your products/services that you offer. Talk about how you started your business, share stories that educate your audience about what&rsquo;s new in your industry, share stories about how your product/service was beneficial to someone or share anything that you think your audience might find interesting (it does not have to be related to your product). This kind of blogging shows that you are an expert in your industry and interested in educating your audience. It sets you apart in the competitive market. This gives you an opportunity to showcase your expertise by educating the visitors and it can turn your audience into buyers.</p><p><strong>Fun Fact:</strong> Did you know that 92% of companies who decided to blog acquired customers through their blog?</p><p><a href=\"http://www.nopcommerce.com/\">nopCommerce</a> is great e-Commerce solution that also offers a variety of CMS features including blog. A store owner has full access for managing the blog posts and related comments.</p>",
											 Tags = "e-commerce, blog, moey",
											 CreatedOnUtc = DateTime.UtcNow,
										},
									new BlogPost
										{
											 AllowComments = true,
											 Language = defaultLanguage,
											 Title = "Why your online store needs a wish list",
											 BodyOverview = "<p>What comes to your mind, when you hear the term&rdquo; wish list&rdquo;? The application of this feature is exactly how it sounds like: a list of things that you wish to get. As an online store owner, would you like your customers to be able to save products in a wish list so that they review or buy them later? Would you like your customers to be able to share their wish list with friends and family for gift giving?</p><p>Offering your customers a feature of wish list as part of shopping cart is a great way to build loyalty to your store site. Having the feature of wish list on a store site allows online businesses to engage with their customers in a smart way as it allows the shoppers to create a list of what they desire and their preferences for future purchase.</p>",
											 Body = "<p>What comes to your mind, when you hear the term&rdquo; wish list&rdquo;? The application of this feature is exactly how it sounds like: a list of things that you wish to get. As an online store owner, would you like your customers to be able to save products in a wish list so that they review or buy them later? Would you like your customers to be able to share their wish list with friends and family for gift giving?</p><p>Offering your customers a feature of wish list as part of shopping cart is a great way to build loyalty to your store site. Having the feature of wish list on a store site allows online businesses to engage with their customers in a smart way as it allows the shoppers to create a list of what they desire and their preferences for future purchase.</p><p>Does every e-Commerce store needs a wish list? The answer to this question in most cases is yes, because of the following reasons:</p><p><strong>Understanding the needs of your customers</strong> - A wish list is a great way to know what is in your customer&rsquo;s mind. Try to think the purchase history as a small portion of the customer&rsquo;s preferences. But, the wish list is like a wide open door that can give any online business a lot of valuable information about their customer and what they like or desire.</p><p><strong>Shoppers like to share their wish list with friends and family</strong> - Providing your customers a way to email their wish list to their friends and family is a pleasant way to make online shopping enjoyable for the shoppers. It is always a good idea to make the wish list sharable by a unique link so that it can be easily shared though different channels like email or on social media sites.</p><p><strong>Wish list can be a great marketing tool</strong> &ndash; Another way to look at wish list is a great marketing tool because it is extremely targeted and the recipients are always motivated to use it. For example: when your younger brother tells you that his wish list is on a certain e-Commerce store. What is the first thing you are going to do? You are most likely to visit the e-Commerce store, check out the wish list and end up buying something for your younger brother.</p><p>So, how a wish list is a marketing tool? The reason is quite simple, it introduce your online store to new customers just how it is explained in the above example.</p><p><strong>Encourage customers to return to the store site</strong> &ndash; Having a feature of wish list on the store site can increase the return traffic because it encourages customers to come back and buy later. Allowing the customers to save the wish list to their online accounts gives them a reason return to the store site and login to the account at any time to view or edit the wish list items.</p><p><strong>Wish list can be used for gifts for different occasions like weddings or birthdays. So, what kind of benefits a gift-giver gets from a wish list?</strong></p><ul><li>It gives them a surety that they didn&rsquo;t buy a wrong gift</li><li>It guarantees that the recipient will like the gift</li><li>It avoids any awkward moments when the recipient unwraps the gift and as a gift-giver you got something that the recipient do not want</li></ul><p><strong>Wish list is a great feature to have on a store site &ndash; So, what kind of benefits a business owner gets from a wish list</strong></p><ul><li>It is a great way to advertise an online store as many people do prefer to shop where their friend or family shop online</li><li>It allows the current customers to return to the store site and open doors for the new customers</li><li>It allows store admins to track what&rsquo;s in customers wish list and run promotions accordingly to target specific customer segments</li></ul><p><a href=\"http://www.nopcommerce.com/\">nopCommerce</a> offers the feature of wish list that allows customers to create a list of products that they desire or planning to buy in future.</p>",
											 Tags = "e-commerce, nopCommerce, sample tag, money",
											 CreatedOnUtc = DateTime.UtcNow.AddSeconds(1),
										},
								};
			_blogPostRepository.Insert(blogPosts);

			//search engine names
			foreach (var blogPost in blogPosts)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = blogPost.Id,
					EntityName = "BlogPost",
					LanguageId = blogPost.LanguageId,
					IsActive = true,
					Slug = blogPost.ValidateSeName("", blogPost.Title, true)
				});
			}

			//comments
			var defaultCustomer = _customerRepository.Table.FirstOrDefault(x => x.Email == defaultUserEmail);
			if (defaultCustomer == null)
				throw new Exception("Cannot load default customer");

			//default store
			var defaultStore = _storeRepository.Table.FirstOrDefault();
			if (defaultStore == null)
				throw new Exception("No default store could be loaded");

			foreach (var blogPost in blogPosts)
			{
				blogPost.BlogComments.Add(new BlogComment
				{
					BlogPostId = blogPost.Id,
					CustomerId = defaultCustomer.Id,
					CommentText = "This is a sample comment for this blog post",
					IsApproved = true,
					StoreId = defaultStore.Id,
					CreatedOnUtc = DateTime.UtcNow
				});
			}
			_blogPostRepository.Update(blogPosts);
		}

		protected virtual void InstallNews(string defaultUserEmail)
		{
			var defaultLanguage = _languageRepository.Table.FirstOrDefault();

			var news = new List<NewsItem>
								{
									new NewsItem
									{
										 AllowComments = true,
										 Language = defaultLanguage,
										 Title = "About nopCommerce",
										 Short = "It's stable and highly usable. From downloads to documentation, www.nopCommerce.com offers a comprehensive base of information, resources, and support to the nopCommerce community.",
										 Full = "<p>For full feature list go to <a href=\"http://www.nopCommerce.com\">nopCommerce.com</a></p><p>Providing outstanding custom search engine optimization, web development services and e-commerce development solutions to our clients at a fair price in a professional manner.</p>",
										 Published  = true,
										 CreatedOnUtc = DateTime.UtcNow,
									},
									new NewsItem
									{
										 AllowComments = true,
										 Language = defaultLanguage,
										 Title = "nopCommerce new release!",
										 Short = "nopCommerce includes everything you need to begin your e-commerce online store. We have thought of everything and it's all included! nopCommerce is a fully customizable shopping cart",
										 Full = "<p>nopCommerce includes everything you need to begin your e-commerce online store. We have thought of everything and it's all included!</p>",
										 Published  = true,
										 CreatedOnUtc = DateTime.UtcNow.AddSeconds(1),
									},
									new NewsItem
									{
										 AllowComments = true,
										 Language = defaultLanguage,
										 Title = "New online store is open!",
										 Short = "The new nopCommerce store is open now! We are very excited to offer our new range of products. We will be constantly adding to our range so please register on our site.",
										 Full = "<p>Our online store is officially up and running. Stock up for the holiday season! We have a great selection of items. We will be constantly adding to our range so please register on our site, this will enable you to keep up to date with any new products.</p><p>All shipping is worldwide and will leave the same day an order is placed! Happy Shopping and spread the word!!</p>",
										 Published  = true,
										 CreatedOnUtc = DateTime.UtcNow.AddSeconds(2),
									},

								};
			_newsItemRepository.Insert(news);

			//search engine names
			foreach (var newsItem in news)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = newsItem.Id,
					EntityName = "NewsItem",
					LanguageId = newsItem.LanguageId,
					IsActive = true,
					Slug = newsItem.ValidateSeName("", newsItem.Title, true)
				});
			}

			//comments
			var defaultCustomer = _customerRepository.Table.FirstOrDefault(x => x.Email == defaultUserEmail);
			if (defaultCustomer == null)
				throw new Exception("Cannot load default customer");

			//default store
			var defaultStore = _storeRepository.Table.FirstOrDefault();
			if (defaultStore == null)
				throw new Exception("No default store could be loaded");

			foreach (var newsItem in news)
			{
				newsItem.NewsComments.Add(new NewsComment
				{
					NewsItemId = newsItem.Id,
					CustomerId = defaultCustomer.Id,
					CommentTitle = "Sample comment title",
					CommentText = "This is a sample comment...",
					IsApproved = true,
					StoreId = defaultStore.Id,
					CreatedOnUtc = DateTime.UtcNow
				});
			}
			_newsItemRepository.Update(news);
		}

		protected virtual void InstallPolls()
		{
			var defaultLanguage = _languageRepository.Table.FirstOrDefault();
			var poll1 = new Poll
			{
				Language = defaultLanguage,
				Name = "Do you like nopCommerce?",
				SystemKeyword = "",
				Published = true,
				ShowOnHomePage = true,
				DisplayOrder = 1,
			};
			poll1.PollAnswers.Add(new PollAnswer
			{
				Name = "Excellent",
				DisplayOrder = 1,
			});
			poll1.PollAnswers.Add(new PollAnswer
			{
				Name = "Good",
				DisplayOrder = 2,
			});
			poll1.PollAnswers.Add(new PollAnswer
			{
				Name = "Poor",
				DisplayOrder = 3,
			});
			poll1.PollAnswers.Add(new PollAnswer
			{
				Name = "Very bad",
				DisplayOrder = 4,
			});
			_pollRepository.Insert(poll1);
		}

		protected virtual void InstallActivityLogTypes()
		{
			var activityLogTypes = new List<ActivityLogType>
			{
                //admin area activities
                new ActivityLogType
				{
					SystemKeyword = "AddNewAddressAttribute",
					Enabled = true,
					Name = "Add a new address attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewAddressAttributeValue",
					Enabled = true,
					Name = "Add a new address attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewAffiliate",
					Enabled = true,
					Name = "Add a new affiliate"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewBlogPost",
					Enabled = true,
					Name = "Add a new blog post"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCampaign",
					Enabled = true,
					Name = "Add a new campaign"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCategory",
					Enabled = true,
					Name = "Add a new category"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCheckoutAttribute",
					Enabled = true,
					Name = "Add a new checkout attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCountry",
					Enabled = true,
					Name = "Add a new country"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCurrency",
					Enabled = true,
					Name = "Add a new currency"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCustomer",
					Enabled = true,
					Name = "Add a new customer"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCustomerAttribute",
					Enabled = true,
					Name = "Add a new customer attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCustomerAttributeValue",
					Enabled = true,
					Name = "Add a new customer attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewCustomerRole",
					Enabled = true,
					Name = "Add a new customer role"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewDiscount",
					Enabled = true,
					Name = "Add a new discount"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewEmailAccount",
					Enabled = true,
					Name = "Add a new email account"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewGiftCard",
					Enabled = true,
					Name = "Add a new gift card"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewLanguage",
					Enabled = true,
					Name = "Add a new language"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewManufacturer",
					Enabled = true,
					Name = "Add a new manufacturer"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewMeasureDimension",
					Enabled = true,
					Name = "Add a new measure dimension"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewMeasureWeight",
					Enabled = true,
					Name = "Add a new measure weight"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewNews",
					Enabled = true,
					Name = "Add a new news"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewProduct",
					Enabled = true,
					Name = "Add a new product"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewProductAttribute",
					Enabled = true,
					Name = "Add a new product attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewSetting",
					Enabled = true,
					Name = "Add a new setting"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewSpecAttribute",
					Enabled = true,
					Name = "Add a new specification attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewStateProvince",
					Enabled = true,
					Name = "Add a new state or province"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewStore",
					Enabled = true,
					Name = "Add a new store"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewTopic",
					Enabled = true,
					Name = "Add a new topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewVendor",
					Enabled = true,
					Name = "Add a new vendor"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewWarehouse",
					Enabled = true,
					Name = "Add a new warehouse"
				},
				new ActivityLogType
				{
					SystemKeyword = "AddNewWidget",
					Enabled = true,
					Name = "Add a new widget"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteActivityLog",
					Enabled = true,
					Name = "Delete activity log"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteAddressAttribute",
					Enabled = true,
					Name = "Delete an address attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteAddressAttributeValue",
					Enabled = true,
					Name = "Delete an address attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteAffiliate",
					Enabled = true,
					Name = "Delete an affiliate"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteBlogPost",
					Enabled = true,
					Name = "Delete a blog post"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteBlogPostComment",
					Enabled = true,
					Name = "Delete a blog post comment"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCampaign",
					Enabled = true,
					Name = "Delete a campaign"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCategory",
					Enabled = true,
					Name = "Delete category"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCheckoutAttribute",
					Enabled = true,
					Name = "Delete a checkout attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCountry",
					Enabled = true,
					Name = "Delete a country"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCurrency",
					Enabled = true,
					Name = "Delete a currency"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCustomer",
					Enabled = true,
					Name = "Delete a customer"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCustomerAttribute",
					Enabled = true,
					Name = "Delete a customer attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCustomerAttributeValue",
					Enabled = true,
					Name = "Delete a customer attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteCustomerRole",
					Enabled = true,
					Name = "Delete a customer role"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteDiscount",
					Enabled = true,
					Name = "Delete a discount"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteEmailAccount",
					Enabled = true,
					Name = "Delete an email account"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteGiftCard",
					Enabled = true,
					Name = "Delete a gift card"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteLanguage",
					Enabled = true,
					Name = "Delete a language"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteManufacturer",
					Enabled = true,
					Name = "Delete a manufacturer"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteMeasureDimension",
					Enabled = true,
					Name = "Delete a measure dimension"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteMeasureWeight",
					Enabled = true,
					Name = "Delete a measure weight"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteMessageTemplate",
					Enabled = true,
					Name = "Delete a message template"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteNews",
					Enabled = true,
					Name = "Delete a news"
				},
				 new ActivityLogType
				{
					SystemKeyword = "DeleteNewsComment",
					Enabled = true,
					Name = "Delete a news comment"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteOrder",
					Enabled = true,
					Name = "Delete an order"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteProduct",
					Enabled = true,
					Name = "Delete a product"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteProductAttribute",
					Enabled = true,
					Name = "Delete a product attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteProductReview",
					Enabled = true,
					Name = "Delete a product review"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteReturnRequest",
					Enabled = true,
					Name = "Delete a return request"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteSetting",
					Enabled = true,
					Name = "Delete a setting"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteSpecAttribute",
					Enabled = true,
					Name = "Delete a specification attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteStateProvince",
					Enabled = true,
					Name = "Delete a state or province"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteStore",
					Enabled = true,
					Name = "Delete a store"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteTopic",
					Enabled = true,
					Name = "Delete a topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteVendor",
					Enabled = true,
					Name = "Delete a vendor"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteWarehouse",
					Enabled = true,
					Name = "Delete a warehouse"
				},
				new ActivityLogType
				{
					SystemKeyword = "DeleteWidget",
					Enabled = true,
					Name = "Delete a widget"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditActivityLogTypes",
					Enabled = true,
					Name = "Edit activity log types"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditAddressAttribute",
					Enabled = true,
					Name = "Edit an address attribute"
				},
				 new ActivityLogType
				{
					SystemKeyword = "EditAddressAttributeValue",
					Enabled = true,
					Name = "Edit an address attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditAffiliate",
					Enabled = true,
					Name = "Edit an affiliate"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditBlogPost",
					Enabled = true,
					Name = "Edit a blog post"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCampaign",
					Enabled = true,
					Name = "Edit a campaign"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCategory",
					Enabled = true,
					Name = "Edit category"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCheckoutAttribute",
					Enabled = true,
					Name = "Edit a checkout attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCountry",
					Enabled = true,
					Name = "Edit a country"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCurrency",
					Enabled = true,
					Name = "Edit a currency"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCustomer",
					Enabled = true,
					Name = "Edit a customer"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCustomerAttribute",
					Enabled = true,
					Name = "Edit a customer attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCustomerAttributeValue",
					Enabled = true,
					Name = "Edit a customer attribute value"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditCustomerRole",
					Enabled = true,
					Name = "Edit a customer role"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditDiscount",
					Enabled = true,
					Name = "Edit a discount"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditEmailAccount",
					Enabled = true,
					Name = "Edit an email account"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditGiftCard",
					Enabled = true,
					Name = "Edit a gift card"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditLanguage",
					Enabled = true,
					Name = "Edit a language"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditManufacturer",
					Enabled = true,
					Name = "Edit a manufacturer"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditMeasureDimension",
					Enabled = true,
					Name = "Edit a measure dimension"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditMeasureWeight",
					Enabled = true,
					Name = "Edit a measure weight"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditMessageTemplate",
					Enabled = true,
					Name = "Edit a message template"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditNews",
					Enabled = true,
					Name = "Edit a news"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditOrder",
					Enabled = true,
					Name = "Edit an order"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditPlugin",
					Enabled = true,
					Name = "Edit a plugin"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditProduct",
					Enabled = true,
					Name = "Edit a product"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditProductAttribute",
					Enabled = true,
					Name = "Edit a product attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditProductReview",
					Enabled = true,
					Name = "Edit a product review"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditPromotionProviders",
					Enabled = true,
					Name = "Edit promotion providers"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditReturnRequest",
					Enabled = true,
					Name = "Edit a return request"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditSettings",
					Enabled = true,
					Name = "Edit setting(s)"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditStateProvince",
					Enabled = true,
					Name = "Edit a state or province"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditStore",
					Enabled = true,
					Name = "Edit a store"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditTask",
					Enabled = true,
					Name = "Edit a task"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditSpecAttribute",
					Enabled = true,
					Name = "Edit a specification attribute"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditVendor",
					Enabled = true,
					Name = "Edit a vendor"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditWarehouse",
					Enabled = true,
					Name = "Edit a warehouse"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditTopic",
					Enabled = true,
					Name = "Edit a topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "EditWidget",
					Enabled = true,
					Name = "Edit a widget"
				},
				new ActivityLogType
				{
					SystemKeyword = "Impersonation.Started",
					Enabled = true,
					Name = "Customer impersonation session. Started"
				},
				new ActivityLogType
				{
					SystemKeyword = "Impersonation.Finished",
					Enabled = true,
					Name = "Customer impersonation session. Finished"
				},
				new ActivityLogType
				{
					SystemKeyword = "InstallNewPlugin",
					Enabled = true,
					Name = "Install a new plugin"
				},
				new ActivityLogType
				{
					SystemKeyword = "UninstallPlugin",
					Enabled = true,
					Name = "Uninstall a plugin"
				},
                //public store activities
                new ActivityLogType
				{
					SystemKeyword = "PublicStore.ViewCategory",
					Enabled = false,
					Name = "Public store. View a category"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.ViewManufacturer",
					Enabled = false,
					Name = "Public store. View a manufacturer"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.ViewProduct",
					Enabled = false,
					Name = "Public store. View a product"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.PlaceOrder",
					Enabled = false,
					Name = "Public store. Place an order"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.SendPM",
					Enabled = false,
					Name = "Public store. Send PM"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.ContactUs",
					Enabled = false,
					Name = "Public store. Use contact us form"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddToCompareList",
					Enabled = false,
					Name = "Public store. Add to compare list"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddToShoppingCart",
					Enabled = false,
					Name = "Public store. Add to shopping cart"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddToWishlist",
					Enabled = false,
					Name = "Public store. Add to wishlist"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.Login",
					Enabled = false,
					Name = "Public store. Login"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.Logout",
					Enabled = false,
					Name = "Public store. Logout"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddProductReview",
					Enabled = false,
					Name = "Public store. Add product review"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddNewsComment",
					Enabled = false,
					Name = "Public store. Add news comment"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddBlogComment",
					Enabled = false,
					Name = "Public store. Add blog comment"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddForumTopic",
					Enabled = false,
					Name = "Public store. Add forum topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.EditForumTopic",
					Enabled = false,
					Name = "Public store. Edit forum topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.DeleteForumTopic",
					Enabled = false,
					Name = "Public store. Delete forum topic"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.AddForumPost",
					Enabled = false,
					Name = "Public store. Add forum post"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.EditForumPost",
					Enabled = false,
					Name = "Public store. Edit forum post"
				},
				new ActivityLogType
				{
					SystemKeyword = "PublicStore.DeleteForumPost",
					Enabled = false,
					Name = "Public store. Delete forum post"
				}
			};
			_activityLogTypeRepository.Insert(activityLogTypes);
		}

		protected virtual void InstallProductTemplates()
		{
			var productTemplates = new List<ProductTemplate>
							   {
								   new ProductTemplate
									   {
										   Name = "Simple product",
										   ViewPath = "ProductTemplate.Simple",
										   DisplayOrder = 10,
										   IgnoredProductTypes = ((int)ProductType.GroupedProduct).ToString()
									   },
								   new ProductTemplate
									   {
										   Name = "Grouped product (with variants)",
										   ViewPath = "ProductTemplate.Grouped",
										   DisplayOrder = 100,
										   IgnoredProductTypes = ((int)ProductType.SimpleProduct).ToString()
									   }
							   };
			_productTemplateRepository.Insert(productTemplates);
		}

		protected virtual void InstallCategoryTemplates()
		{
			var categoryTemplates = new List<CategoryTemplate>
							   {
								   new CategoryTemplate
									   {
										   Name = "Products in Grid or Lines",
										   ViewPath = "MBN.CategoryTemplate.ProductsInGridOrLines",
										   DisplayOrder = 1
									   },
							   };
			_categoryTemplateRepository.Insert(categoryTemplates);
		}

		protected virtual void InstallManufacturerTemplates()
		{
			var manufacturerTemplates = new List<ManufacturerTemplate>
							   {
								   new ManufacturerTemplate
									   {
										   Name = "Products in Grid or Lines",
										   ViewPath = "ManufacturerTemplate.ProductsInGridOrLines",
										   DisplayOrder = 1
									   },
							   };
			_manufacturerTemplateRepository.Insert(manufacturerTemplates);
		}

		protected virtual void InstallTopicTemplates()
		{
			var topicTemplates = new List<TopicTemplate>
							   {
								   new TopicTemplate
									   {
										   Name = "Default template",
										   ViewPath = "TopicDetails",
										   DisplayOrder = 1
									   },
							   };
			_topicTemplateRepository.Insert(topicTemplates);
		}

		protected virtual void InstallScheduleTasks()
		{
			var tasks = new List<ScheduleTask>
			{
				new ScheduleTask
				{
					Name = "Send emails",
					Seconds = 60,
					Type = "Nop.Services.Messages.QueuedMessagesSendTask, Nop.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask
				{
					Name = "Keep alive",
					Seconds = 300,
					Type = "Nop.Services.Common.KeepAliveTask, Nop.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask
				{
					Name = "Delete guests",
					Seconds = 600,
					Type = "Nop.Services.Customers.DeleteGuestsTask, Nop.Services",
					Enabled = true,
					StopOnError = false,
				},
				new ScheduleTask
				{
					Name = "Clear cache",
					Seconds = 600,
					Type = "Nop.Services.Caching.ClearCacheTask, Nop.Services",
					Enabled = false,
					StopOnError = false,
				},
				new ScheduleTask
				{
					Name = "Clear log",
                    //60 minutes
                    Seconds = 3600,
					Type = "Nop.Services.Logging.ClearLogTask, Nop.Services",
					Enabled = false,
					StopOnError = false,
				},
				new ScheduleTask
				{
					Name = "Update currency exchange rates",
                    //60 minutes
                    Seconds = 3600,
					Type = "Nop.Services.Directory.UpdateExchangeRateTask, Nop.Services",
					Enabled = true,
					StopOnError = false,
				},
			};

			_scheduleTaskRepository.Insert(tasks);
		}

		protected virtual void InstallReturnRequestReasons()
		{
			var returnRequestReasons = new List<ReturnRequestReason>
								{
									new ReturnRequestReason
										{
											Name = "Received Wrong Product",
											DisplayOrder = 1
										},
									new ReturnRequestReason
										{
											Name = "Wrong Product Ordered",
											DisplayOrder = 2
										},
									new ReturnRequestReason
										{
											Name = "There Was A Problem With The Product",
											DisplayOrder = 3
										}
								};
			_returnRequestReasonRepository.Insert(returnRequestReasons);
		}
		protected virtual void InstallReturnRequestActions()
		{
			var returnRequestActions = new List<ReturnRequestAction>
								{
									new ReturnRequestAction
										{
											Name = "Repair",
											DisplayOrder = 1
										},
									new ReturnRequestAction
										{
											Name = "Replacement",
											DisplayOrder = 2
										},
									new ReturnRequestAction
										{
											Name = "Store Credit",
											DisplayOrder = 3
										}
								};
			_returnRequestActionRepository.Insert(returnRequestActions);
		}

		protected virtual void InstallWarehouses()
		{
			var warehouse1address = new Address
			{
				Address1 = "21 West 52nd Street",
				City = "New York",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "New York"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				ZipPostalCode = "10021",
				CreatedOnUtc = DateTime.UtcNow,
			};
			_addressRepository.Insert(warehouse1address);
			var warehouse2address = new Address
			{
				Address1 = "300 South Spring Stree",
				City = "Los Angeles",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "California"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				ZipPostalCode = "90013",
				CreatedOnUtc = DateTime.UtcNow,
			};
			_addressRepository.Insert(warehouse2address);
			var warehouses = new List<Warehouse>
			{
				new Warehouse
				{
					Name = "Warehouse 1 (New York)",
					AddressId = warehouse1address.Id
				},
				new Warehouse
				{
					Name = "Warehouse 2 (Los Angeles)",
					AddressId = warehouse2address.Id
				}
			};

			_warehouseRepository.Insert(warehouses);
		}

		protected virtual void InstallVendors()
		{
			var vendors = new List<Vendor>
			{
				new Vendor
				{
					Name = "Vendor 1",
					Email = "vendor1email@gmail.com",
					Description = "Some description...",
					AdminComment = "",
					PictureId = 0,
					Active = true,
					DisplayOrder = 1,
					PageSize = 6,
					AllowCustomersToSelectPageSize = true,
					PageSizeOptions = "6, 3, 9, 18",
				},
				new Vendor
				{
					Name = "Vendor 2",
					Email = "vendor2email@gmail.com",
					Description = "Some description...",
					AdminComment = "",
					PictureId = 0,
					Active = true,
					DisplayOrder = 2,
					PageSize = 6,
					AllowCustomersToSelectPageSize = true,
					PageSizeOptions = "6, 3, 9, 18",
				}
			};

			_vendorRepository.Insert(vendors);

			//search engine names
			foreach (var vendor in vendors)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = vendor.Id,
					EntityName = "Vendor",
					LanguageId = 0,
					IsActive = true,
					Slug = vendor.ValidateSeName("", vendor.Name, true)
				});
			}
		}

		protected virtual void InstallAffiliates()
		{
			var affiliateAddress = new Address
			{
				FirstName = "John",
				LastName = "Smith",
				Email = "affiliate_email@gmail.com",
				Company = "Company name here...",
				City = "New York",
				Address1 = "21 West 52nd Street",
				ZipPostalCode = "10021",
				PhoneNumber = "123456789",
				StateProvince = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "New York"),
				Country = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA"),
				CreatedOnUtc = DateTime.UtcNow,
			};
			_addressRepository.Insert(affiliateAddress);
			var affilate = new Affiliate
			{
				Active = true,
				Address = affiliateAddress
			};
			_affiliateRepository.Insert(affilate);
		}

		private void AddProductTag(Product product, string tag)
		{
			var productTag = _productTagRepository.Table.FirstOrDefault(pt => pt.Name == tag);
			if (productTag == null)
			{
				productTag = new ProductTag
				{
					Name = tag,
				};
			}
			product.ProductTags.Add(productTag);
			_productRepository.Update(product);
		}

		#endregion

		#region Methods

		public virtual void InstallData(string defaultUserEmail,
			string defaultUserPassword, bool installSampleData = true)
		{
			InstallStores();
			InstallMeasures();
			InstallTaxCategories();
			InstallLanguages();
			InstallCurrencies();
			InstallCountriesAndStates();
			InstallShippingMethods();
			InstallDeliveryDates();
			InstallProductAvailabilityRanges();
			InstallCustomersAndUsers(defaultUserEmail, defaultUserPassword);
			InstallEmailAccounts();
			InstallMessageTemplates();
			InstallSettings();
			InstallTopicTemplates();
			InstallTopics();
			InstallLocaleResources();
			InstallActivityLogTypes();
			InstallProductTemplates();
			InstallCategoryTemplates();
			InstallManufacturerTemplates();
			InstallScheduleTasks();
			InstallReturnRequestReasons();
			InstallReturnRequestActions();

			if (installSampleData)
			{
				InstallCheckoutAttributes();
				InstallSpecificationAttributes();
				InstallProductAttributes();
				InstallCategories();
				InstallManufacturers();
				InstallProducts(defaultUserEmail);
				InstallForums();
				InstallDiscounts();
				InstallBlogPosts(defaultUserEmail);
				InstallNews(defaultUserEmail);
				InstallPolls();
				InstallWarehouses();
				InstallVendors();
				InstallAffiliates();
				InstallOrders();
				InstallActivityLog(defaultUserEmail);
				InstallSearchTerms();
			}
		}

		#endregion
	}
}