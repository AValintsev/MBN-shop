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

			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

			var homePageTopic = new Topic
			{
				SystemName = "HomePageText",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				DisplayOrder = 1,
				Published = true,
				Title = "Электроника - это будущее",
				Body = "<p>Наш интернет магазин познакомит Вас с деятельностью в инновационной сфере - добывание криптовалюты, а также и с другими инновационными технологиями.</p>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var contactUsTopic = new Topic
			{
				SystemName = "ContactUs",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				DisplayOrder = 1,
				IncludeInTopMenu = true,
				Published = true,
				Title = "Контакты",
				Body = @"<p><a href=""tel:+380634203770""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(63) 420 37 70</a><br />
							<a href=""tel:+380674682128""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(67) 468 21 28</a><br />
							<a href=""tel:+380504103760""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(50) 410 37 60</a><br /></p>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var aboutUsTopic = new Topic
			{
				SystemName = "AboutUs",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				IncludeInFooterColumn1 = true,
				IncludeInTopMenu = true,
				DisplayOrder = 20,
				Published = true,
				Title = "О нас",
				Body = "<p></p>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var registerGuestTopic = new Topic
			{
				SystemName = "CheckoutAsGuestOrRegister",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				DisplayOrder = 1,
				Published = true,
				Title = "",
				Body = "<p><strong>Зарегестрируйтесь для экономии Вашего времени!</strong>Зарегестрировавшись, Вы сможете:</p><ul><li>Покупать легко и быстро</li><li>Просматривать историю ваших заказов и их статус</li></ul>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var pageNotFoundTopic = new Topic
			{
				SystemName = "PageNotFound",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				DisplayOrder = 1,
				Published = true,
				Title = "Страница не найдена",
				Body = "<p><strong>Запрошенная вами страница не найдена, и у нас есть догадка почему.</strong></p><ul><li>Если Вы ввели URL адрес вручную, пожалуйста, проверьте нет ли опечаток.</li><li> Страница больше не существует. В этом случае мы извиняемся за неудобства.</li></ul>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var conditionsOfUseTopic = new Topic
			{
				SystemName = "ConditionsOfUse",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				IncludeInFooterColumn1 = true,
				IncludeInTopMenu = true,
				DisplayOrder = 15,
				Published = true,
				Title = "Пользовательское соглашение",
				Body = @"<h3>1. Условия использования сайта</h3>
<p>1.1. Данное пользовательское соглашение (именуемое в дальнейшем Соглашение) представляет собой оферту условий по пользованию веб-сайтом mbnpro.com.ua (далее – Сайт), в лице Администрации сайта и физическим лицом (именуемым в дальнейшем Пользователь), и регламентирующее условия предоставления Пользователем информации для размещения на Сайте.</p>
<p>1.2. Пользователем Сайта считается любое физическое лицо, когда-либо осуществившее доступ к Сайту, достигшее возраста, допустимого для акцепта настоящего Соглашения.</p>
<p>1.3. Пользователь обязан полностью ознакомиться с настоящим Соглашением до момента регистрации на Сайте. Регистрация Пользователя на Сайте означает полное и безоговорочное принятие Пользователем настоящего Соглашения. В случае несогласия с условиями Соглашения, использование Сайта Пользователем должно быть немедленно прекращено.</p>
<p>1.4. Настоящее Соглашение может быть изменено и/или дополнено Администрацией Сайта в одностороннем порядке без какого-либо специального уведомления. Настоящие Правила являются открытым и общедоступным документом.</p>
<p>1.5. Соглашение предусматривает взаимные права и обязанности Пользователя и Администрации Сайта по следующим пунктам:</p>
<ul>
<p></p>
<h3>2. Порядок использования Сайта</h3>
<p>2.1. Сайт позволяет вам просматривать и загружать информацию с нашего сайта исключительно для личного некоммерческого использования. Запрещается изменять материалы сайта, распространять для общественных или коммерческих целей. Любое использование информации на других сайтах или в компьютерных сетях запрещается.</p>
<p>2.2. При регистрации на сайте и оформлении заказов, вы соглашаетесь предоставить достоверную и точную информацию о себе и своих контактных данных.</p>
<p>2.3. В процессе регистрации на сайте, вы получаете логин и пароль за безопасность, которых вы несете ответственность.</p>
<p>2.4. Вы можете обращаться к нам с вопросами, претензиями, пожеланиями по улучшению работы, либо с какой-либо иной информацией. При этом вы несете ответственность, что данное обращение не является незаконным, угрожающим, нарушает авторские права, дискриминацию людей по какому-либо признаку, а также содержит оскорбления либо иным образом нарушает действующее законодательство Украины.</p>
<p></p>
<h3>3. Персональная информация Пользователя</h3>
<p>3.1. Администрация сайта с уважением и ответственностью относится к конфиденциальной информации любого лица, ставшего посетителем этого сайта. Принимая это Соглашение Пользователь дает согласие на сбор и использование определенной информации о Пользователе в соответствии с положениями Закона Украины «О защите персональных данных» и политикой Администрации сайта о защите персональных данных. Кроме того, пользователь дает согласие, что Администрация сайта может собирать, использовать, передавать, обрабатывать и поддерживать информацию, связанную с аккаунтом Пользователя с целью предоставления соответственных услуг.</p>
<p>3.2. Администрация сайта обязуеться осуществлять сбор только той персональной информации, которую Потребитель предоставляет добровольно в случае, когда информация нужна для предоставления (улучшения) услуг Потребителю.</p>
<p>3.3. Администрация сайта собирает как основные персональные данные, такие как имя, фамилия, отчество, адрес и электронный адрес, так и вторичные (технические) данные - файлы cookies, информация о соединениях и системная информация.</p>
<p>3.4. Пользователь соглашается с тем, что конфиденциальность переданных через Интернет данных не гарантирована в случае, если доступ к этим данным получен третьими лицами вне зоны технический средств связи, подвластных Администрации сайта, Администрация сайта не несет ответственности за ущерб, нанесенный таким доступом.</p>
<p>3.5. Администрация сайта может использовать любую собранную через Сайт информацию с целью улучшения содержания интернет-сайта, его доработки, передачи информации Пользователю (по запросам), для маркетинговых или исследовательских целей, а также для других целей, не противоречащим положениям действующего законодательства Украины.</p>
<p></p>
<h3>4. Ограничение ответственности Администрации сайта</h3>
<p>4.1. Администрация сайта не несет никакой ответственности за любые ошибки, опечатки и неточности, которые могут быть обнаружены в материалах, содержащихся на данном Сайте. Администрация сайта прикладывает все необходимые усилия, чтобы обеспечить точность и достоверность представляемой на Сайте информации. Вся информация и материалы предоставляются на условиях ""как есть"", без каких либо гарантий, как явных, так и подразумеваемых.</p>
<p>4.2. Информация на Сайте постоянно обновляется и в любой момент может стать устаревшей. Администрация сайта не несет ответственности за получение устаревшей информации с Сайта, а также за неспособность Пользователя получить обновления хранящейся на Сайте информации.</p>
<p>4.3. Администрация сайта не несет никакой ответственности за высказывания и мнения посетителей сайта, оставленные в качестве комментариев или обзоров. Мнение Администрация сайта может не совпадать с мнением и позицией авторов обзоров и комментариев. В то же время Администрация сайта принимает все возможные меры, чтобы не допускать публикацию сообщений, нарушающих действующее законодательство или нормы морали.</p>
<p>4.4. Администрация сайта не несет ответственности за возможные противоправные действия Пользователя относительно третьих лиц, либо третьих лиц относительно Пользователя.</p>
<p>4.5. Администрация сайта не несет ответственности за высказывания Пользователя, произведенные или опубликованные на Сайте.</p>
<p>4.6. Администрация сайта не несет ответственности за ущерб, убытки или расходы (реальные либо возможные), возникшие в связи с настоящим Сайтом, его использованием или невозможностью использования.</p>
<p>4.7. Администрация сайта не несет ответственности за утерю Пользователем возможности доступа к своему аккаунту — учетной записи на Сайте.</p>
<p>4.8. Администрация сайта не несет ответственности за неполное, неточное, некорректное указание Пользователем своих данных при создании учетной записи Пользователя.</p>
<p>4.9. При возникновении проблем в использовании Сайта, несогласия с конкретными разделами Пользовательского соглашения, либо получении Пользователем недостоверной информации от третьих лиц, либо информации оскорбительного характера, любой иной неприемлемой информации, пожалуйста, обратитесь к администрации Сайта для того, чтобы Администрация сайта могла проанализировать и устранить соответствующие дефекты, ограничить и предотвратить поступление на Сайт нежелательной информации, а также, при необходимости, ограничить либо прекратить обязательства по предоставлению своих услуг любому Пользователю и клиенту, умышленно нарушающему предписания Соглашения и функционирование работы Сайта.</p>
<p>4.10. В целях вышеизложенного Администрация сайта оставляет за собой право удалять размещенную на Сайте информацию и предпринимать технические и юридические меры для прекращения доступа к Сайту Пользователей, создающих согласно заключению Администрация сайта, проблемы в использовании Сайта другими Пользователями, или Пользователей, нарушающих требования Соглашения.</p>
<p></p>
<h3>5. Порядок действия Соглашения</h3>
<p>5.1. Настоящее Соглашение является договором. Администрация сайта оставляет за собой право как изменить настоящее Соглашение, так и ввести новое. Подобные изменения вступают в силу с момента их размещения на Сайте. Использование Пользователем материалов сайта после изменения Соглашения автоматически означает их принятие.</p>
<p>5.2. Данное Соглашение вступает в силу при первом посещении Сайта Пользователем и действует между Пользователем и Компанией на протяжении всего периода использования Пользователем Сайта.</p>
<p>5.3. Сайт является объектом права интеллектуальной собственности Администрации сайта. Все исключительные имущественные авторские права на сайт принадлежат Администрации сайта. Использование сайта Пользователями возможно строго в рамках Соглашения и законодательства Украины о правах интеллектуальной собственности.</p>
<p>5.4. Все торговые марки и названия, на которые даются ссылки в материалах настоящего Cайта, являются собственностью их соответствующих владельцев.</p>
<p>5.5. Пользователь соглашается не воспроизводить, не повторять, не копировать, какие-либо части Сайта, кроме тех случаев, когда такое разрешение дано Пользователю Администрацией сайта.</p>
<p>5.6. Настоящее Соглашение регулируется и толкуется в соответствии с законодательством Украины. Вопросы, не урегулированные Соглашением, подлежат разрешению в соответствии с законодательством Украины.</p>",
				TopicTemplateId = defaultTopicTemplate.Id
			};
			var shippingInfo = new Topic
			{
				SystemName = "ShippingInfo",
				IncludeInSitemap = false,
				IsPasswordProtected = false,
				IncludeInFooterColumn1 = true,
				IncludeInTopMenu = true,
				DisplayOrder = 5,
				Published = true,
				Title = "Оплата и доставка",
				Body = @"<p><strong><span style=""font-size:large"">Варианты оплаты:</span></strong></p>
	 <ul class=""disc"">
<li>- наложенный платеж при отправке Новой Почтой;</li>
<li>- оплата при получении;</li>
<li>- предоплата.&nbsp;</li>
</ul>
<p><span style=""font-size:large;""><strong>Доставка товара производится:</strong></span></p>
<ul class=""disc"">
<li>- перевозчиком<span style=""color: #0000ff;""><a href = ""http://novaposhta.ua/timetable"" target=""_blank""><span style=""color: #0000ff;""> Новая Почта</span></a></span> (1-2 дня);</li>
<li>- доступен самовывоз из нашего офиса;</li>
</ul>
<p>&nbsp;</p>
<p><span style=""color: #ff0000;""> Услуги доставки Новой Почты, а также стоимость отправки наложенного платежа(2% от суммы наложенного платежа + 20грн) оплачивает клиент.</span></p>",

				TopicTemplateId = defaultTopicTemplate.Id
			};

			var topics = new List<Topic>
							   {
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
										   SystemName = "LoginRegistrationInfo",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = false,
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
										   Published = false,
										   Title = "Privacy notice",
										   Body = "<p>Put your privacy policy information here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   new Topic
									   {
										   SystemName = "ApplyVendor",
										   IncludeInSitemap = false,
										   IsPasswordProtected = false,
										   DisplayOrder = 1,
										   Published = false,
										   Title = "",
										   Body = "<p>Put your apply vendor instructions here. You can edit this in the admin site.</p>",
										   TopicTemplateId = defaultTopicTemplate.Id
									   },
								   aboutUsTopic,
								   homePageTopic,
								   contactUsTopic,
								   registerGuestTopic,
								   pageNotFoundTopic,
								   conditionsOfUseTopic,
								   shippingInfo
							   };
			_topicRepository.Insert(topics);

			_localizedEntityService.SaveLocalizedValue(homePageTopic, c => c.Title, "Електроніка - це майбутнє", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(homePageTopic,
				c => c.Body,
				"<p>Наш інтернет магазин познайомить вас із діяльністю в інноваційній сфері добування криптовалюти, а також з іншими новітніми технологіями.</p>",
				languageUa.Id);

			_localizedEntityService.SaveLocalizedValue(contactUsTopic, c => c.Title, "Контакти", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(contactUsTopic, c => c.Body,
				@"<p><a href=""tel:+380634203770""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(63) 420 37 70</a><br />
							<a href=""tel:+380674682128""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(67) 468 21 28</a><br />
							<a href=""tel:+380504103760""><i class=""fa fa-phone"" aria-hidden=""true""></i> +380(50) 410 37 60</a><br /></p>",
				languageUa.Id);

			_localizedEntityService.SaveLocalizedValue(aboutUsTopic, c => c.Title, "Про нас", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(aboutUsTopic,
				c => c.Body,
				"<p></p>",
				languageUa.Id);

			_localizedEntityService.SaveLocalizedValue(registerGuestTopic,
				c => c.Body,
				"<p><strong>Зарегестрируйтесь для экономии Вашего времени!</strong>Зарегестрировавшись, Вы сможете:</p><ul><li>Покупать легко и быстро</li><li>Просматривать историю ваших заказов и их статус</li></ul>",
				languageUa.Id);

			_localizedEntityService.SaveLocalizedValue(pageNotFoundTopic, c => c.Title, "Сторінку не знайдена", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(pageNotFoundTopic,
				c => c.Body,
				"<p><strong>Сторінку не знайдено, і в нас є здогадка чому.</strong></p><ul><li>Якщо Ви вводили URL адресу вручну, будь ласка, перевірте чи немає помилки.</li><li>Сторінка більше не існує. В цьому випадку ми вибачаємося за незручності.</li></ul>",
				languageUa.Id);

			_localizedEntityService.SaveLocalizedValue(shippingInfo, c => c.Title, "Оплата та доставка", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(shippingInfo,
				c => c.Body,
				 @"<p><strong><span style=""font-size:large"">Варіанти оплати:</span></strong></p>
	 <ul class=""disc"">
<li>- наложений платіж при відправці Новою Поштою;</li>
<li>- оплата готівкою при отриманні товару;</li>
<li>- передоплата;</li>
</ul>
<p><span style=""font-size:large;""><strong>Доставка товару здійснюється:</strong></span></p>
<ul class=""disc"">
<li>- перевізником<span style=""color: #0000ff;""><a href = ""http://novaposhta.ua/timetable"" target=""_blank""><span style=""color: #0000ff;""> Нова Пошта</span></a></span> (1-2 дня);</li>
<li>- доступний самовивіз з нашого офісу;</li>
</ul>
<p>&nbsp;</p>
<p><span style=""color: #ff0000;""> Послуги доставки Нової Пошті, а також вартість відправки наложеного платіжу (2% від сумі наложеного платіжу + 20грн) сплачує клієнт.</span></p>"
, languageUa.Id);

			#region condition of use
			_localizedEntityService.SaveLocalizedValue(conditionsOfUseTopic, c => c.Title, "Користувацька угода", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(conditionsOfUseTopic,
				c => c.Body,
				@"<h3>
<strong>1. Умови використання сайту</strong>
</h3>
<p>1.1. Дана угода користувача (іменована надалі Угода) являє собою оферту умов по користуванню веб-сайтом mbnpro.com.ua (далі - Сайт), в особі Адміністрації сайту та фізичною особою (іменованою надалі Користувач), і регламентує умови надання Користувачем інформації для розміщення на Сайті.</p>
<p>1.2. Користувачем Сайту вважається будь-яка фізична особа, яка будь-коли здійснила доступ до Сайту, яка досягла віку, допустимого для акцепту цієї Угоди.</p>
<p>1.3. Користувач зобов'язаний повністю ознайомитися з цією Угодою до моменту реєстрації на Сайті. Реєстрація Користувача на Сайті означає повне і беззастережне прийняття Користувачем цієї Угоди. У разі незгоди з умовами Угоди, використання Сайту Користувачем повинно бути негайно припинено.</p>
<p>1.4. Ця Угода може бути змінена та/або доповнена Адміністрацією Сайту в односторонньому порядку без будь-якого спеціального повідомлення. Ці Правила є відкритим і загальнодоступним документом.</p>
<p>1.5. Угода передбачає взаємні права та обов'язки Користувача і Адміністрації Сайту за наступними пунктами:</p>
<ul>
<h3>
<p>2.1. Сайт дозволяє вам переглядати і завантажувати інформацію з нашого сайту виключно для особистого некомерційного використання. Забороняється змінювати матеріали сайту, поширювати для громадських або комерційних цілей. Будь-яке використання інформації на інших сайтах або в комп'ютерних мережах забороняється.</p>
<p>2.2. При реєстрації на сайті та оформленні замовлень, ви погоджуєтеся надати достовірну і точну інформацію про себе та свої контактні дані.</p>
<p>2.3. У процесі реєстрації на сайті, ви отримуєте логін і пароль за безпеку, яких ви несете відповідальність.</p>
<p>2.4. Ви можете звертатися до нас з питаннями, претензіями, побажаннями щодо поліпшення роботи, або з будь-якої іншою інформацією. При цьому ви несете відповідальність, що дане звернення не є незаконним, загрозливим, порушує авторські права, дискримінацію людей за будь-якою ознакою, а також містить образи чи іншим чином порушує чинне законодавство України.</p>
<p></p>
<h3>
<p>3.1. Адміністрація сайту з повагою і відповідальністю ставиться до конфіденційної інформації будь-якої особи, що стала відвідувачем цього сайту. Зважаючи на це Угода Користувач дає згоду на збір і використання певної інформації про Користувача відповідно до положень Закону України «Про захист персональних даних» та політики Адміністрації сайту про захист персональних даних. Крім того, користувач дає згоду, що Адміністрація сайту може збирати, використовувати, передавати, обробляти і підтримувати інформацію, пов'язану з членством Користувача з метою надання відповідних послуг.</p>
<p>3.2. Адміністрація сайту зобов'язуєтеся здійснювати збір тільки тієї персональної інформації, яку Споживач надає добровільно в разі, коли інформація потрібна для надання (поліпшення) послуг Споживачу.</p>
<p>3.3. Адміністрація сайту збирає як основні персональні дані, такі як ім'я, прізвище, по батькові, адреса та електронну адресу, так і вторинні (технічні) дані - файли cookies, інформація про з'єднання і системна інформація.</p>
<p>3.4. Користувач погоджується з тим, що конфіденційність переданих через Інтернет даних не гарантована в разі, якщо доступ до цих даних отримано третіми особами поза зоною технічний засобів зв'язку, підвладних Адміністрації сайту, Адміністрація сайту не несе відповідальності за шкоду, завдану таким доступом.</p>
<p>3.5. Адміністрація сайту може використовувати будь-яку зібрану через Сайт інформацію з метою поліпшення змісту інтернет-сайту, його доопрацювання, передачі інформації Користувачеві (за запитами), для маркетингових або дослідницьких цілей, а також для інших цілей, що не суперечить положенням чинного законодавства України.</p>
<p></p>
<h3>
<p>4.1. Адміністрація сайту не несе ніякої відповідальності за будь-які помилки, друкарські помилки і неточності, які можуть бути виявлені в матеріалах, що містяться на даному Сайті. Адміністрація сайту докладає всіх необхідних зусиль, щоб забезпечити точність і достовірність на Сайті інформації. Вся інформація та матеріали надаються на умовах ""як є"", без будь-яких гарантій, як явних, так і непрямих.</p>
<p>4.2. Інформація на Сайті постійно оновлюється і в будь-який момент може стати застарілою. Адміністрація сайту не несе відповідальності за отримання застарілої інформації з сайту, а також за нездатність Користувача отримати оновлення збережуваної на Сайті інформації.</p>
<p>4.3. Адміністрація сайту не несе ніякої відповідальності за висловлювання і думки відвідувачів сайту, залишені в якості коментарів або оглядів. Думка редакцiї може не збігатися з думкою і позицією авторів оглядів і коментарів. У той же час Адміністрація сайту вживає всіх можливих заходів, щоб не допускати публікацію повідомлень, які порушують чинне законодавство або норми моралі.</p>
<p>4.4. Адміністрація сайту не несе відповідальності за можливі протиправні дії Користувача щодо третіх осіб, або третіх осіб щодо Користувача.</p>
<p>4.5. Адміністрація сайту не несе відповідальності за висловлювання Користувача, зроблені або опубліковані на Сайті.</p>
<p>4.6. Адміністрація сайту не несе відповідальності за шкоду, збитки або витрати (реальні або можливі), що виникли в зв'язку з цим Сайтом, його використанням або неможливістю використання.</p>
<p>4.7. Адміністрація сайту не несе відповідальності за втрату Користувачем можливості доступу до свого облікового запису - облікового запису на Сайті.</p>
<p>4.8. Адміністрація сайту не несе відповідальності за неповне, неточне, некоректне вказування Користувачем своїх даних при створенні облікового запису Користувача.</p>
<p>4.9. При виникненні проблем у використанні Сайту, незгоди з конкретними розділами Угоди, або отриманні Користувачем недостовірної інформації від третіх осіб, або інформації образливого характеру, будь-якої іншої неприйнятної інформації, будь ласка, зверніться до адміністрації сайту для того, щоб Адміністрація сайту могла проаналізувати і усунути відповідні дефекти, обмежити і запобігти надходженню на Сайт небажаної інформації, а також, при необхідності, обмежити або припинити зобов'язання з надання своїх послуг будь-якому Користувачеві і клієнту, який навмисне порушує приписи Угоди і функціонування роботи Сайту.</p>
<p>4.10. З огляду на вищевикладене Адміністрація сайту залишає за собою право видаляти розміщену на Сайті інформацію і робити технічні і юридичні заходи для припинення доступу до Сайту користувачів, що створюють згідно з висновком Адміністрація сайту, проблеми у використанні Сайту іншими Користувачами, або користувачів, які порушують вимоги Угоди.</p>
<p></p>
<h3>5. Порядок дії Угоди</h3>
<p>5.1. Ця Угода є договором. Адміністрація сайту залишає за собою право як змінити цю Угоду, так і ввести нову. Подібні зміни вступають в силу з моменту їх розміщення на Сайті. Використання Користувачем матеріалів сайту після зміни Угоди автоматично означає їх прийняття.</p>
<p>5.2. Дана Угода вступає в силу при першому відвідуванні Сайту Користувачем і діє між Користувачем та Адміністрацією протягом усього періоду використання Сайту Користувачем .</p>
<p>5.3. Сайт є об'єктом права інтелектуальної власності Адміністрації сайту. Всі виключні майнові авторські права на сайт належать Адміністрації сайту. Використання сайту Користувачами можливо строго в рамках Угоди і законодавства України про права інтелектуальної власності.</p>
<p>5.4. Всі торгові марки і назви, на які даються посилання в матеріалах цього сайту, є власністю їх відповідних власників.</p>
<p>5.5. Користувач погоджується не відтворювати, не повторювати, не копіювати, будь-які частини Сайту, крім тих випадків, коли такий дозвіл дано Користувачеві Адміністрацією сайту.</p>
<p>5.6. Ця Угода регулюється і тлумачиться відповідно до законодавства України. Питання, не врегульовані Угодою, підлягають вирішенню відповідно до законодавства України.</p>",
				languageUa.Id);
			#endregion

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
						"Payments.PayInStore",
						"Payments.CashOnDelivery"
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
			var _localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

			#region Vendor
			var vendor = new SpecificationAttribute
			{
				Name = "Вендор",
				DisplayOrder = 1,
				SpecificationAttributeOptions = {
					new SpecificationAttributeOption
					{
						Name = "AMD",
						DisplayOrder = 1
					},
					new SpecificationAttributeOption
					{
						Name = "NVIDIA",
						DisplayOrder = 1
					}
				}
			};

			#endregion

			#region Graphic chip
			var graphicChip = new SpecificationAttribute
			{
				Name = "Графический чип",
				DisplayOrder = 1,
				SpecificationAttributeOptions =
				{
					new SpecificationAttributeOption
					{
						Name = "RX 470",
						DisplayOrder = 1
					},
					new SpecificationAttributeOption
					{
						Name = "RX 480",
						DisplayOrder = 2
					}
				}
			};
			#endregion

			#region RAM Size
			var ramSize = new SpecificationAttribute
			{
				Name = "Обьём памяти",
				DisplayOrder = 2,
				SpecificationAttributeOptions =
				{
					new SpecificationAttributeOption
					{
						Name = "4 Гб",
						DisplayOrder = 1
					},
					new SpecificationAttributeOption
					{
						Name = "6 Гб",
						DisplayOrder = 2
					},
					new SpecificationAttributeOption
					{
						Name = "8 Гб",
						DisplayOrder = 3
					}
				}
			};
			#endregion

			#region Socket
			var socket = new SpecificationAttribute
			{
				Name = "Сокет",
				DisplayOrder = 3,
				SpecificationAttributeOptions =
				{
					new SpecificationAttributeOption
					{
						Name = "Socket 1150",
						DisplayOrder = 1
					},
					new SpecificationAttributeOption
					{
						Name = "Socket 1151",
						DisplayOrder = 2
					},
					new SpecificationAttributeOption
					{
						Name = "Socket 1155",
						DisplayOrder = 3
					},
					new SpecificationAttributeOption
					{
						Name = "Socket FM2",
						DisplayOrder = 4
					},
					new SpecificationAttributeOption
					{
						Name = "Socket FM2+",
						DisplayOrder = 5
					},
					new SpecificationAttributeOption
					{
						Name = "Socket AM3",
						DisplayOrder = 6
					}
				}
			};
			#endregion

			#region Form factor
			var formFactor = new SpecificationAttribute
			{
				Name = "Форм-фактор",
				DisplayOrder = 4,
				SpecificationAttributeOptions =
				{
					new SpecificationAttributeOption
					{
						Name = "ATX",
						DisplayOrder = 1
					},
					new SpecificationAttributeOption
					{
						Name = "microATX",
						DisplayOrder = 2
					},
					new SpecificationAttributeOption
					{
						Name = "mini-ITX",
						DisplayOrder = 3
					},
					new SpecificationAttributeOption
					{
						Name = "EATX",
						DisplayOrder = 3
					}
				}
			};
			#endregion

			var specificationAttributes = new List<SpecificationAttribute>
								{
									vendor, graphicChip, ramSize, socket, formFactor
								};
			_specificationAttributeRepository.Insert(specificationAttributes);

			_localizedEntityService.SaveLocalizedValue(vendor, c => c.Name, "Вендор", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(graphicChip, c => c.Name, "Графічний чіп", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(ramSize, c => c.Name, "Об'єм пам'яті", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(socket, c => c.Name, "Сокет", languageUa.Id);
			_localizedEntityService.SaveLocalizedValue(formFactor, c => c.Name, "Форм-фактор", languageUa.Id);
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

			//default store
			var defaultStore = _storeRepository.Table.FirstOrDefault();
			if (defaultStore == null)
				throw new Exception("No default store could be loaded");


			//pictures
			var pictureService = EngineContext.Current.Resolve<IPictureService>();
			var sampleImagesPath = CommonHelper.MapPath("~/content/samples/");

			////downloads
			//var downloadService = EngineContext.Current.Resolve<IDownloadService>();
			//var sampleDownloadsPath = CommonHelper.MapPath("~/content/samples/");

			//products
			var allProducts = new List<Product>();

			#region Rx470
			var rx470 = new Product
			{
				ProductType = ProductType.SimpleProduct,
				VisibleIndividually = true,
				Name = "SAPPHIRE NITRO RX 470 4G D5 1236/7000MHz",
				Sku = "11256-10-20G",
				ShortDescription = "",
				FullDescription = @"<p>Частота графического чипа - 1236МГц<br/>
Частота памяти - 7000МГц<br/>
Порты: DVI - D, 2xHDMI, 2xDisplayPort<br/>
  Рекомендуемый блок питания 500Вт<br/>
  Дополнительное питание: 8pin<br/>
  Гарантия 12 месяцев + 12 месяцев гарантийного обслуживания </p>",
				ProductTemplateId = productTemplateSimple.Id,
				AllowCustomerReviews = true,
				Price = 205M,
				IsShipEnabled = true,
				Weight = 7,
				Length = 7,
				Width = 7,
				Height = 7,
				ManageInventoryMethod = ManageInventoryMethod.ManageStock,
				StockQuantity = 10000,
				NotifyAdminForQuantityBelow = 1,
				AllowBackInStockSubscriptions = false,
				DisplayStockAvailability = true,
				LowStockActivity = LowStockActivity.DisableBuyButton,
				BackorderMode = BackorderMode.NoBackorders,
				OrderMinimumQuantity = 1,
				OrderMaximumQuantity = 10000,
				Published = true,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
				ProductCategories =
				{
					new ProductCategory
					{
						Category = _categoryRepository.Table.First(c => c.Name == "Видеокарты"),
						DisplayOrder = 1,
					}
				},
				ProductSpecificationAttributes =
				{
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 1,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Вендор")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "AMD")
					},
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 2,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Графический чип")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "RX 470")
					},
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 3,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Обьём памяти")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "4 Гб")
					}
				}
			};
			allProducts.Add(rx470);
			rx470.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "rx470/1.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(rx470.Name)),
				DisplayOrder = 1,
			});
			rx470.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "rx470/2.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(rx470.Name)),
				DisplayOrder = 2,
			});
			rx470.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "rx470/3.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(rx470.Name)),
				DisplayOrder = 3,
			});
			rx470.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "rx470/4.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(rx470.Name)),
				DisplayOrder = 4,
			});
			_productRepository.Insert(rx470);
			#endregion

			#region RX480
			var RX480 = new Product
			{
				ProductType = ProductType.SimpleProduct,
				VisibleIndividually = true,
				Name = "SAPPHIRE NITRO RX 480 4G D5 1236/7000MHz",
				Sku = "11256-10-20G",
				ShortDescription = "",
				FullDescription = @"<p>Частота графического чипа - 1236МГц<br/>
Частота памяти - 7000МГц<br/>
Порты: DVI - D, 2xHDMI, 2xDisplayPort<br/>
  Рекомендуемый блок питания 500Вт<br/>
  Дополнительное питание: 8pin<br/>
  Гарантия 12 месяцев + 12 месяцев гарантийного обслуживания </p>",
				ProductTemplateId = productTemplateSimple.Id,
				AllowCustomerReviews = true,
				Price = 245M,
				IsShipEnabled = true,
				Weight = 7,
				Length = 7,
				Width = 7,
				Height = 7,
				ManageInventoryMethod = ManageInventoryMethod.ManageStock,
				StockQuantity = 10000,
				NotifyAdminForQuantityBelow = 1,
				AllowBackInStockSubscriptions = false,
				DisplayStockAvailability = true,
				LowStockActivity = LowStockActivity.DisableBuyButton,
				BackorderMode = BackorderMode.NoBackorders,
				OrderMinimumQuantity = 1,
				OrderMaximumQuantity = 10000,
				Published = true,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
				ProductCategories =
				{
					new ProductCategory
					{
						Category = _categoryRepository.Table.First(c => c.Name == "Видеокарты"),
						DisplayOrder = 1,
					}
				},
				ProductSpecificationAttributes =
				{
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 1,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Вендор")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "AMD")
					},
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 2,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Графический чип")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "RX 480")
					},
					new ProductSpecificationAttribute
					{
						AllowFiltering = true,
						ShowOnProductPage = true,
						DisplayOrder = 3,
						SpecificationAttributeOption = _specificationAttributeRepository.Table.Single(sa => sa.Name == "Обьём памяти")
							.SpecificationAttributeOptions.Single(sao => sao.Name == "4 Гб")
					}
				}
			};
			allProducts.Add(RX480);
			RX480.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "RX480/1.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(RX480.Name)),
				DisplayOrder = 1,
			});
			RX480.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "RX480/2.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(RX480.Name)),
				DisplayOrder = 2,
			});
			RX480.ProductPictures.Add(new ProductPicture
			{
				Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "RX480/3.jpg"), MimeTypes.ImageJpeg, pictureService.GetPictureSeName(RX480.Name)),
				DisplayOrder = 3,
			});
			_productRepository.Insert(RX480);
			#endregion


			//search engine names
			foreach (var product in allProducts)
			{
				_urlRecordRepository.Insert(new UrlRecord
				{
					EntityId = product.Id,
					EntityName = "Product",
					LanguageId = 0,
					IsActive = true,
					Slug = product.ValidateSeName("", product.Name, true)
				});
			}

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
			//var defaultLanguage = _languageRepository.Table.FirstOrDefault();
			//var poll1 = new Poll
			//{
			//	Language = defaultLanguage,
			//	Name = "Do you like nopCommerce?",
			//	SystemKeyword = "",
			//	Published = true,
			//	ShowOnHomePage = true,
			//	DisplayOrder = 1,
			//};
			//poll1.PollAnswers.Add(new PollAnswer
			//{
			//	Name = "Excellent",
			//	DisplayOrder = 1,
			//});
			//poll1.PollAnswers.Add(new PollAnswer
			//{
			//	Name = "Good",
			//	DisplayOrder = 2,
			//});
			//poll1.PollAnswers.Add(new PollAnswer
			//{
			//	Name = "Poor",
			//	DisplayOrder = 3,
			//});
			//poll1.PollAnswers.Add(new PollAnswer
			//{
			//	Name = "Very bad",
			//	DisplayOrder = 4,
			//});
			//_pollRepository.Insert(poll1);
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