/**
 * Flexify 1.1 | jQuery Plugin
 *
 * Copyright (c) 2008 Tim Cameron Ryan
 * http://plugins.jquery.com/project/Flexify
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
 
(function ($) {
	/*
	 * Flexification
	 */
	 
	$.fn.flexify = function ()
	{		
		// find flexible elements
		var elements = this.add(this.find('*')), flexDimensions = {}, flexPositioned = {};
		$.each(['horizontal', 'vertical'], function (i, axis) {
			// the parents of flexible dimension elements
			flexDimensions[axis] = elements.filter(function () {
				return $(this).data('flexify-parent') && $(this).data('flexify-parent').axes[axis];
			});
			// flexibly positioned elements
			flexPositioned[axis] = elements.filter(function () {
				return $(this).data('flexify-position') && $(this).data('flexify-position').axes[axis];
			});
		});
		
		// flexification function
		var closure = this;
		function flex()
		{
			// flex horizontal
			flexDimensions.horizontal.flexifyDimensions('horizontal');
			flexPositioned.horizontal.flexifyPosition('horizontal');
			// cache current width
			var width = closure.dimension('horizontal');
			// reset vertical
			flexDimensions.vertical.flexifyDimensions('vertical');
			flexPositioned.vertical.flexifyPosition('vertical');
			// reflexify if vertical scrollbar has disappeared
			if (width < closure.dimension('horizontal'))
				return flex();
				
			// add resize handler
			$(window).one('reflow', flex);
		}		
		// initial call
		flex();	
		
		// continue object chain
		return this;
	}

	$.fn.flexifyDimensions = function (axis)
	{
		// reset stage (iterate backwards)
		for (var i = this.length - 1, parent = this[i]; i > -1; parent = this[--i])
		{
			// reset flexible styles and calculate minimum flex unit size
			var data = $(parent).data('flexify-parent'), flexUnit = [0];
			data.children[axis].each(function resetChildFlexAndGetMinimums() {
				var childData = $(this).data('flexify-child');
				
				// iterate flexible properties
				for (var property in childData.properties[axis])
					$(this).css(property, 0);
					
				// reset flexible dimensions and check flex unit
				if (childData.dimensions.hasOwnProperty(axis))
				{
					$(this).dimension(axis, 'content', childData.intrinsic[axis]);
					flexUnit.push((parseInt(childData.intrinsic[axis]) || $(this).dimension(axis, 'content')) / childData.dimensions[axis]);
				}
			});
			
			// normalize flex unit
			data.flexUnit = Math.max.apply(null, flexUnit);
			// equalize flexible sizes
			data.children[axis].each(function equalizeFlexChild() {
				// set minimum flex size
				var childData = $(this).data('flexify-child');
				for (var property in childData.properties[axis])
					$(this).css(property, data.flexUnit * childData.properties[axis][property])
				if (childData.dimensions.hasOwnProperty(axis))
					$(this).dimension(axis, 'content', data.flexUnit * childData.dimensions[axis]);
			});

			// trigger handler
			$(parent).trigger('FlexifyReset', [axis]);
		}
		
		// apply stage (iterates forwards)
		for (var i = 0, parent = this[i]; i < this.length; parent = this[++i])
		{
			// calculate space
			var data = $(parent).data('flexify-parent');
			var freeSpace = $(parent).dimension(axis, 'content'), usedSpace;
			if (axis == $(parent).flow())
				usedSpace = $(parent).contentDimension(axis, 'offset');

			// iterate flexible children
			data.children[axis].each(function applyFlexToChild(i, child) {			
				// calculate used space
				if (axis != $(parent).flow())
//[TODO] this should be using 'margin', but safari adds phantom margins to fixed-width elements
					usedSpace = $(this).dimension(axis, 'border');

				var childData = $(this).data('flexify-child');
				// calculate flexible space
				var flexUnit = data.flexUnit + Math.max(Math.floor((freeSpace - usedSpace) / (axis == $(parent).flow() ? data.childDivisor[axis] : childData.divisor[axis])), 0);

				// iterate flexible properties
				for (var property in childData.properties[axis])
					$(this).css(property, flexUnit * childData.properties[axis][property]);
				if (childData.dimensions.hasOwnProperty(axis))
					$(this).dimension(axis, 'content', flexUnit * childData.dimensions[axis]);
			});

			// trigger handler
			$(parent).trigger('FlexifyApply', [axis]);
		}
		
		// continue object chain
		return this;
	}
	
	$.fn.flexifyPosition = function (axis)
	{
		for (var i = 0, child = this[i], parent; i < this.length; child = this[++i])
		{
			// check parent
			if (!(parent = child.offsetParent))
				return;
				
			// get divisor
			var data = $(child).data('flexify-position'), divisor = 0;
			for (var property in data.properties[axis])
				divisor += data.properties[axis][property];				
			// calculate flexible space
			var flexUnit = Math.max(($(parent).dimension(axis, 'border') - $(child).dimension(axis, 'border')) / divisor, 0);
			// iterate properties
			for (var property in data.properties[axis])
				$(child).css(property, data.properties[axis][property] * flexUnit);
		}
		
		// continue object chain
		return this;
	}
	
	/*
	 * Flex property
	 */

	$.fn.flex = function (property, value)
	{	
		// rewrite border properties
		property = property.replace(/^(border-[a-z]+)$/, '$1-width');
		// property lists
		var flexibleProps = {
			horizontal: 'margin-left|margin-right|border-left-width|border-right-width|padding-left|padding-right|width|left|right'.split('|'),
			vertical: 'margin-top|margin-bottom|border-top-width|border-bottom-width|padding-top|padding-bottom|height|top|bottom'.split('|')
		};
		var positionProps = 'left|right|top|bottom'.split('|');
		var dimensionProps = 'width|height'.split('|');

		// get dimension axis
		for (var axis in flexibleProps)
			if ($.inArray(property, flexibleProps[axis]) != -1)
				break;
		if ($.inArray(property, flexibleProps[axis]) == -1)
			return false;
		
		// getter
		if (arguments.length < 2)
			return $.inArray(property, dimensionProps) != -1 ?
			    $(this).data('flexify-child').dimensions[axis] :
			    $.inArray(property, positionProps) != -1 ?
			    $(this).data('flexify-position')[axis][property] :
			    $(this).data('flexify-child').properties[axis][property];
		
		// setter for position
		if ($.inArray(property, positionProps) != -1)
		{
			return this.each(function (i, child) {
				// initialize data
				if (!$(this).data('flexify-position'))
					$(this).data('flexify-position', {
					    properties: {horizontal: {}, vertical: {}},
					    axes: {horizontal: false, vertical: false}
					});
				// set axis flag and add property
				$(this).data('flexify-position').axes[axis] = true;
				$(this).data('flexify-position').properties[axis][property] = value;
			});
		}
		
		//[FIX] flexifying documentElement/body in some browsers causes issues
		var flexFilter = $.browser.msie ? 'html' :
		    $.browser.opera && $.browser.version < 9.5 ? 'html, body' : '';
		// setter for dimension
		return this.not(flexFilter).each(function (i, child) {
			// flexible parents must have flow
			if (!$(this).parent().flow())
				$(this).parent().flow('vertical');
		
			// initialize parent data
			if (!$(this).parent().data('flexify-parent'))
			    $(this).parent().data('flexify-parent', {
				flexUnit: 0,
				children: {horizontal: $([]), vertical: $([])},
				childDivisor: {horizontal: 0, vertical: 0},
				axes: {horizontal: false, vertical: false}
			    });			    
			// initialize child data
			if (!$(this).data('flexify-child'))
			    $(this).data('flexify-child', {
				divisor: {horizontal: 0, vertical: 0},
				intrinsic: {horizontal: null, vertical: null},
				properties: {horizontal: {}, vertical: {}},
				dimensions: {}
			    });
		
			// add to free space divisor
			$(this).data('flexify-child').divisor[axis] += value;
			$(this).parent().data('flexify-parent').childDivisor[axis] += value;
			// set axis flag and append to parent's children array
			$(this).parent().data('flexify-parent').axes[axis] = true;
			$(this).parent().data('flexify-parent').children[axis] = $(this).parent().data('flexify-parent').children[axis].add(this);
			
			// save dimension property and its intrinsic size
			if (property == 'width' || property == 'height')
			{
				$(this).data('flexify-child').dimensions[axis] = value;
				$(this).data('flexify-child').intrinsic[axis] = this.style[property];
				
				//[FIX] opera 9.0 has to manually reset form element dimensions
				// because reading body.clientWidth prevents textarea width: auto from working!?
				if ($.browser.opera && $.browser.version < 9.5 && $(this).is(':button, input, textarea, iframe'))
					$(this).data('flexify-child').intrinsic[axis] = $(this).dimension(axis);
			}
			// save property
			else
				$(this).data('flexify-child').properties[axis][property] = value;
			
			// add flexify reset handler to define (just once!) horizontal flow minimums
//[TODO] could this be called every time? it would prevent float jogging on initial resize
			var parentChain = $(this).add($(this).parents());
			$(this).parent().unbind('FlexifyReset.setMinimums')
			    .one('FlexifyReset.setMinimums', function (e, axis) {
				// only on horizontal axis
				if (axis == 'horizontal')
					parentChain.trigger('ResetFlow');
			});
		});
	}
	
	// :flexible selector
	 
	function flexSelector(elem, i, selector) {
		return $(elem).data('flexify-child');
	}
	$.extend($.expr[':'], {flexible: flexSelector});

	/*
	 * Reflow event
	 */
	
	// window resize polling function
	//[FIX] we create our own window resize event, because:
	// Mozilla is too slow.
	// IE can't immediately resetFlex in a resize event thread.
	var wWidth, wHeight;
	function pollWindowResize() {
		var flag = wWidth != $(window).dimension('horizontal') || wHeight != $(window).dimension('vertical');
		wWidth = $(window).dimension('horizontal');
		wHeight = $(window).dimension('vertical');
		return flag;
	}
	// getting window width requires the DOM to be loaded
	$(function () {
		// initial call
		pollWindowResize();
		// check window resize at set intervals
		setInterval(function () {
			if (pollWindowResize())
				$(window).trigger('reflow');
		}, 250);
	});
	
//[TODO] font resize/page zoom
	
	/*
	 * Flow property
	 */

	$.fn.flow = function flow(axis)
	{
		// getter
		if (arguments.length < 1)
			return this.data('flow');
	
		// setter
		return this.each(function (i, parent) {
			// initialize flow box
			$(this).data('flow', axis == 'horizontal' ? 'horizontal' : 'vertical');

			// flow can only work with elements; remove whitespace and wrap child text nodes
			for (var child = this.firstChild; child; child = child.nextSibling)
				if (child.nodeType == 3)
					/^\s+$/.test(child.nodeValue) ? this.removeChild(child)
					    : $(child).wrap('<div class="text"></div>');
			//[FIX] flow children generally need layout in IE to check dimensions
			$(this).children().css('zoom', 1);
			
			// horizontal flow requires some changes
			if (axis == 'horizontal')
			{
				// filter children widths
				var shrinkWrap = $([]);
				$(this).children().each(function () {
					// non-replaced elements of width: auto are shrink-wrapped
					if (!$(this).is(':button') && $(this).dimensionIsAuto(axis))
						// overfloW : auto has no intrinsic width
						$(this).dimension(axis, 'content',
						    $(this).css('overflow') == 'auto' ? 0 :
						    $(this).contentDimension(axis, 'scroll'));
				});
				
				// overflow: auto to contain floats (not hidden, because of <body> tag!)
				//[FIX] this screws up IE (naturally), so force layout instead
				$(this).css($.browser.msie ? {'zoom': 1} : {'overflow': 'auto'});
				// float: left for horizontal row
				$(this).children().css({'float': 'left'});

				// add minimum-setting handler
				$(this).bind('ResetFlow', function () {
					$.swap(this, {width: '10000px'}, function () {
						//[FIX] just give a bit of extra wiggle room for Mozilla
						$(this).dimension(axis, 'min', $(this).contentDimension(axis, 'offset') + 1);
					});
				}).trigger('ResetFlow');
			}
		});
	}
	
	// :vertical, :horizontal selectors
	 
	function flowSelector(elem, i, selector) {
		return $(elem).data('flow') == selector[2];
	}
	$.extend($.expr[':'], {vertical: flowSelector, horizontal: flowSelector});
	
	/*
	 * Dimensions
	 */
	
	// adapted from code by Mike Helgeson
	// http://dev.jquery.com/ticket/3082
	(function ()
	{
		// get numeric attr value of the local element
//[TODO] a private curCSS would speed up num() reading
		function num(elem, attr)
		{
			return parseFloat($.curCSS( elem, attr, true)) || 0;
		}
		
		// properties
		var Prop = {horizontal: 'Width', vertical: 'Height'},
		    tl = {horizontal: 'Left', vertical: 'Top'}, br = {horizontal: 'Right', vertical: 'Bottom'};
		    
		/*
		 * Dimensions
		 */
		
		// get/set dimension
		$.fn.dimension = function (axis, prefix, value)
		{
			// check that there be any elements
			if (!this.length)
				return null;
			var elem = this[0];
			// default prefix
			prefix = prefix || 'content';
			
			// for document/window size, use <html> or <body> (depending on Quirks vs Standards mode)
			if (elem == window || elem == document)
				return (document.compatMode == 'CSS1Compat' ? document.documentElement : document.body)['client' + Prop[axis]];
			// get the dimensions of a single element
			if (value == undefined)
			{
				// using offset dimension
				if (prefix == 'margin' || prefix == 'border')
					value = elem['offset' + Prop[axis]]
					    + (prefix == 'margin' ? num(elem, 'margin' + tl[axis]) + num(elem, 'margin' + br[axis]) : 0);
				// using client dimension
				if (prefix == 'padding' || prefix == 'content')
					// element size from padding to padding
					value = elem['client' + Prop[axis]]
					    - (prefix == 'content' ? num(elem, 'padding' + tl[axis]) + num(elem, 'padding' + br[axis]) : 0);
					
				return Math.max(0, Math.ceil(value));					    
			}
				
			// set value via jQuery function
			return this[prefix == 'content' ? Prop[axis].toLowerCase() : prefix + Prop[axis]](value); 
		}
		
		//[FIX] in IE, the <html> element has a default border of medium
		// rather than support border word values in .dimension(), just rewrite this to '2px'
		if ($.browser.msie)
		{
			$(function () {
				var html = document.documentElement;
				$.each(['Left', 'Right', 'Top', 'Bottom'], function (i, prop) {
					if (html.currentStyle['border' + prop + 'Style'] != 'none' &&
					    html.currentStyle['border' + prop + 'Width'] == 'medium')
						html.style['border' + prop + 'Width'] = '2px';
				});
			});
		}
		
		/*
		 * Content dimensions
		 */
		
		// get content dimension
		$.fn.contentDimension = function (axis, means)
		{
			// check that there be any elements
			if (!this.length)
				return null;
			var elem = this[0];
			
			//[FIX] in IE, scrollHeight always has this value; this also avoids other bugs
			if ($.browser.msie && axis == 'vertical')
				return this[0].scrollHeight - (this[0] != document ? 
				    num(this[0], 'padding' + tl[axis]) - num(this[0], 'padding' + br[axis]) : 0);
		
			// get dimension by scroll
			if (means == 'scroll')
			{
				// shrink element and use overflow to determine children size
				var css = {'float': 'left', 'border': '2px solid black'}, val;
				//[FIX] Safari doesn't like dimensions of '0'
				css[Prop[axis].toLowerCase()] = '1px';
				$.swap(elem, css, function () {
					val = elem['scroll' + Prop[axis]] - num(elem, 'padding' + tl[axis]) - num(elem, 'padding' + br[axis]);
				});
				return Math.max(0, Math.ceil(val));
			}
			
			// get dimension by offset between first and last elements
			for (var firstChild = elem.firstChild;
			    firstChild && firstChild.nodeType != 1;
			    firstChild = firstChild.nextSibling);
			if (!firstChild)
				return null;
			for (var lastChild = elem.lastChild;
			    lastChild && lastChild.nodeType != 1;
			    lastChild = lastChild.previousSibling);
			// calculate offset difference and add last element's height
			return Math.max(0, Math.ceil(
				num(firstChild, 'margin' + tl[axis])
				+ (lastChild['offset' + tl[axis]] - firstChild['offset' + tl[axis]]) + lastChild['offset' + Prop[axis]]
				+ num(lastChild, 'margin' + br[axis])
			    ));
		}
		
		/*
		 * Dimension analysis
		 */
		
		// check if a dimension is set to 'auto'
		$.fn.dimensionIsAuto = function (axis)
		{
			// check that there be any elements
			if (!this.length)
				return;
			// auto will not expand offset dimension with padding
			var oldPadding = this[0].style['padding' + tl[axis]];
			this[0].style['padding' + tl[axis]] = 0;
			var dimension = this[0]['offset' + Prop[axis]];
			this[0].style['padding' + tl[axis]] = '1px';
			var flag = this[0]['offset' + Prop[axis]] == dimension;
			this[0].style['padding' + tl[axis]] = oldPadding;
			return flag;
		}
	})();
	
	/*
	 * Min-width/height
	 */
	
	// min-height and width setters
	$.each({Height: 'height', Width: 'width'}, function (Prop, prop)
	{		
		$.fn['min' + Prop] = function (value)
		{
			// set minimum dimension property
			return this.css('min' + Prop, typeof value == 'string' ? value : value + 'px');
		}
	});
	
	//[FIX] add min-width to IE6
	if ($.browser.msie && $.browser.version < 7)
	{
		// replace min-width and min-height functions
		$.each({Height: 'height', Width: 'width'}, function (Prop, prop)
		{		
			$.fn['min' + Prop] = function (value)
			{
				return this.each(function (i, child) {
					// add reflow handler
					$(window).bind('reflow.minDimensions', function () {
						child.runtimeStyle[prop] = '';
					});
					// trigger hasLayout and add resize handler
					$(this).css('zoom', 1).unbind('resize.minDimensions').bind('resize.minDimensions', function () {
						if ($(this)[prop]() < parseInt(value))
							this.runtimeStyle[prop] = typeof value == 'string' ? value : value + 'px';
					}).trigger('resize.minDimensions');
				});
			}
			
			// body tag is an exception
			$(window).one('resize.minDimensions', function checkBodyResize() {
				$('body').trigger('resize.minDimensions');
				$(window).one('resize.minDimensions', checkBodyResize);
			});
		});
	}
})(jQuery);
