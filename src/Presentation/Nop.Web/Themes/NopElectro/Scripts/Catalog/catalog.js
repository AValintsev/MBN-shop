;
var catalog = (function () {
	var shopName = '';
	var priceFormat = '';
	var $priceRange;

	function _load(url) {
		$.ajax({
			type: "GET",
			url: url,
			success: function (response) {
				$('.master-column-wrapper').html(response);
				catalog.refreshPriceRange();
			},
			error: function (error) {
				alert('error on the page')
			}
		});
	}

	function _replaceUrlParam(url, paramName, paramValue) {
		if (paramValue == null)
			paramValue = '';
		var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)')
		if (url.search(pattern) >= 0) {
			return url.replace(pattern, '$1' + paramValue + '$2');
		}
		return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue
	}

	function _bindEvents() {
		$(document).on('click', '.product-filters a', function (evt) {
			evt.preventDefault();
			History.pushState(null, document.title, $(this).attr('href'));
		});

		$(document).on('click', '.product-viewmode a', function (evt) {
			evt.preventDefault();
			History.pushState(null, document.title, $(this).attr('href'));
		});

		$(document).on('change', '.product-selectors select', function (evt) {
			evt.preventDefault();
			History.pushState(null, document.title, $(this).val());
			return false;
		});
	}

	function _initPriceRange() {
		if (!document.getElementById('priceRange')) return;

		$priceRange = new Slider("#priceRange");

		$priceRange.on('slide', function (newValue) {
			$('#startPrice').html(numeral(newValue[0]).format(priceFormat));
			$('#endPrice').html(numeral(newValue[1]).format(priceFormat));
		});

		$priceRange.on('slideStop', function (newValue) {
			var state = History.getState();
			var priceValue = newValue[0] + '-' + newValue[1];
			var newUrl = _replaceUrlParam(state.url, 'price', priceValue);

			History.pushState(null, document.title, newUrl);
		});

	}

	function _init() {
		shopName = document.title.substr(0, document.title.indexOf('.')) + '. ';
		priceFormat = $('#priceFormat').val();

		_initPriceRange();

		_bindEvents();

		History.Adapter.bind(window, 'statechange', function () {
			var state = History.getState();
			_load(state.url);
		});
	}

	return {
		init: _init,
		refreshPriceRange: _initPriceRange
	}
})();

$(function () { catalog.init(); });