/* Gumby JS */

(function ($) {

	var Gumby = function () {

		// Gumby data object for storing simpleUI classes and handlers 
		var gumbyData = {},

			setGumbyData = function (key, value) {
				return gumbyData[key] = value;
			},

			getGumbyData = function (key) {
				return gumbyData[key] || false;
			},

			/**
			 * Simple UI Elements
			 * ------------------
			 * UI elements that bind to an event, toggle 
			 * a class with possibility to run simple logic 
			 * on completion and test for specific conditions
			 */
			simpleUI = {

				// simple UI elements holder object
				ui: [
				
					// checkbox - check/uncheck (including hidden input) on click
					{
						selector: '.checkbox',
						onEvent: 'click',
						className: 'checked',
						target: false,
						condition: false,
						// check/uncheck hidden checkbox input
						complete: function ($e) {
							var checked = $e.hasClass('checked');
							$e.children('input').attr('checked', checked);
						}
					},

					// radio - check/uncheck (including hidden input) on click 
					// also uncheck all others with same name
					{
						selector: '.radio',
						onEvent: 'click',
						className: 'checked',
						target: false,
						condition: false,
						// check hidden radio input and uncheck others
						complete: function ($e) {
							var $input = $e.children('input'),
								// radio buttons with matching names in the same group
								$otherInputs = $('input[name="' + $input.attr('name') + '"]');

							// ensure other radio buttons are not checked
							$otherInputs.attr('checked', false).parent().removeClass('checked');

							// check this one
							$input.attr('checked', true).parent().addClass('checked');
						}
					},	

					// validation - add/remove error class dependent on value being present
					// conditional method used to check for value
					{
						selector: 'form[data-form="validate"] .field',
						onEvent: 'blur',
						className: 'error',
						target: false,
						// check input is required and if so add/remove error class based on present value
						condition: function ($e) { 
							var $child = $e.find('input, textarea').first(),
								val = $child.val();

							if(!$child.attr('required')) {
								return false;
							}

							// email/regular validation
							if (($child.attr('type') === 'email' && !val.match(/^[\w.%&=+$#!-']+@[\w.-]+\.[a-zA-Z]{2,4}$/)) || !val.length) {
								$e.addClass('error');
								return false;
							}
							
							$e.removeClass('error');
						},
						complete: false
					},

					// toggles - toggle active class on itself and selector in data-for
					// on click
					{
						selector: '.toggle:not([data-on]), .toggle[data-on="click"]',
						onEvent: 'click',
						className: 'active',
						target: function($e) {
							return $e.add($($e.attr('data-for')));
						},
						condition: false,
						complete: false
					},

					// on mouseover (will always add class) and mouseout (will always remove class)
					{
						selector: '.toggle[data-on="hover"]',
						onEvent: 'mouseover mouseout',
						className: 'active',
						target: function($e) {
							return $e.add($($e.attr('data-for')));
						},
						condition: false,
						complete: false
					}
				],

				// initialize simple UI
				init: function () {

					var x, ui, $e, callBack, conditionalCallBack, activeClass, targetName;

					// loop round gumby UI elements applying active/inactive class logic
					for (x in simpleUI.ui) {

						ui = simpleUI.ui[x];
						$e = $(ui.selector);
						// complete call back
						callBack = ui.complete && typeof ui.complete === 'function' ? ui.complete : false;
						// conditional callback
						conditionalCallBack = ui.condition && typeof ui.condition === 'function' ? ui.condition : false;
						targetName = ui.target && typeof ui.target === 'function' ? ui.target : false;
						activeClass = ui.className || false;

						// store UI data
						// replace spaces with dashes for GumbyData object reference
						setGumbyData(ui.selector.replace(' ', '-'), {
							'GumbyCallback' : callBack,
							'GumbyConditionalCallBack' : conditionalCallBack,
							'GumbyActiveClass' : activeClass,
							'GumbyTarget' : targetName
						});

						// bind it all!
						$(document).on(ui.onEvent, ui.selector, function (e) {
							e.preventDefault();

							var $this = $(this),
								$target = $(this),
								gumbyData = getGumbyData(e.handleObj.selector.replace(' ', '-')),
								condition = true;

							// if there is a conditional function test it here
							// leaving if it returns false
							if(gumbyData.GumbyConditionalCallBack) {
								return condition = gumbyData.GumbyConditionalCallBack($this);
							}

							// no conditional or it passed so toggle class
							if (gumbyData.GumbyActiveClass) {
								// check for sepcified target
								if(gumbyData.GumbyTarget) {
									$target = gumbyData.GumbyTarget($this);
								}
								$target.toggleClass(gumbyData.GumbyActiveClass);
							}

							// if complete call back present call it here
							if (gumbyData.GumbyCallback) {
								gumbyData.GumbyCallback($this);
							}
						});
					}
				}
			},

			/**
			 * Complex UI Elements
			 * ------------------
			 * UI elements that require logic passed the
			 * capabilities of simple add/remove class.
			 */
			complexUI = {

				// init separate complexUI elements
				init: function () {
					complexUI.pickers();
					complexUI.skipLinks();
					complexUI.tabs();
				},

				// pickers - open picker on click and update <select> and picker label when option chosen
				pickers: function() {

					// open picker on click
					$(document).on('click', '.picker', function (e) {
						e.preventDefault();

						var $this = $(this),
							openTimer = null;

						// custom .picker style are removed on handheld devices using :after to insert hidden content and inform JS 
						if (window.getComputedStyle($this.get(0), ':after').getPropertyValue('content') === 'handheld') {
							return false;
						}

						// mouseout for > 500ms will close picker
						$this.hover(function () {
							clearTimeout(openTimer);
						}, function () {
							var $this = $(this);
							openTimer = setTimeout(function () {
								$this.removeClass('open');
							}, 500);
						});

						$this.toggleClass('open');
					});

					// clicking children elements should update hidden <select> and .picker active label
					$(document).on('click', '.picker > ul > li', function (e) {
						e.preventDefault();

						var $this = $(this),
							$parent = $this.parents('.picker'),
							val = $this.children('a').html();

						// update label
						$parent.children('.toggle').html(val + '<span class="caret"></span>');

						// update hidden select
						$parent.find('option').attr('selected', false).eq($this.index()  +  1).attr('selected', true);
					});
				},

				// skiplinks - slide to data-type content area on click of skiplink and on window load if hash present
				skipLinks: function () {
					var skip = function () {

						var skipTypeParts,
							skipType,
							$skipTos = $('[data-type]'),
							$skipTo = false,
							onWin = false,
							$this = $(this);

						if ($this.get(0) === window  && !window.location.hash) {
							return false;
						}

						// initial load skip
						if ($this.get(0) === window && window.location.hash) {
							skipType = window.location.hash.replace('#', '');
							onWin = true;
						} else {
							skipTypeParts = $this.attr('href').split('#');
							skipType = skipTypeParts[skipTypeParts.length - 1];
						}

						// loop round potential data-type matches
						$skipTos.each(function () {
							// data-type can be multiple space separated values
							var typeParts = $(this).attr('data-type').split(' '), x;

							// find first match and break the each
							for (x in typeParts) {
								if (typeParts[x] === skipType) {
									$skipTo = $(this);
									return false;
								}
							}
						});

						if (!$skipTo.length) {
							return false;
						}

						// scroll to skiplink 
						$('body,html').animate({
							'scrollTop' : $skipTo.offset().top
						}, 350);

						// update hash if  not an initial hash load
						if (onWin) {
							window.location.hash = skipType;
						}

					};

					// bind to skip links and window load
					$(document).on('click', '.skiplink a, .skipnav ul li a, .skip', skip);
					$(window).load(skip);
				},

				// tabs - activate tab and tab content on click as well as on window load if hash present
				tabs: function () {

					var activateTab = function ($tab) {
						var // this links tabs set
							$tabs = $tab.parents('.tabs'),
							// currently active tab
							activeTab = {
								'tab' : $tabs.find('ul').children('li.active'),
								'content' : $tabs.find('div[data-tab].active')
							},
							// newly clicked tab
							newTab = {
								'tab' : $tab.parent('li'),
								'content' : $tabs.find('[data-tab=' + $tab.attr('href').replace('#', '') + ']')
							},
							x, y;

						// remove active class from tab and content
						for (x in activeTab) {
							activeTab[x].removeClass('active');
						}

						// add active class to tab and content
						for (y in newTab) {
							newTab[y].addClass('active');
						}
					}

					// hook up tab links
					$(document).on('click', '.tabs ul li a', function(e) {
						activateTab($(this));
					});

					// hook up initial load active tab 
					if (window.location.hash) {
						var $activeTab = $('a[href="'  +  window.location.hash  +  '"]');
						if ($activeTab.length && $activeTab.parents('.tabs').length) {
							activateTab($activeTab);
						}
					}
				}
			},

			// initialize Gumby
			init = function () {
				simpleUI.init();
				complexUI.init();
			};

		// return public methods
		return {
			i: init
		}
	}().i();

})(window.jQuery);

