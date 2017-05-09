var sliderManager = (function () {
	var removeUrl = '';
	function _bindEvents() {
		$('input[remove-slide]').on('click', function () {
			var $btn = $(this);
			var id = $btn.attr('remove-slide');

			$.ajax({
				url: '/Plugins/WidgetsNivoSliderLocalized/DeleteSlide',
				type: 'POST',
				data: { id: id, name: "deleteslide" },
				success: function () {
					$btn.parents('.panel').hide("slow", function () { $(this).remove(); })
				}
			});
		});
	}

	function _init() {
		_bindEvents();
	}

	return {
		init: _init
	}

})();

$(function () { sliderManager.init(); });
