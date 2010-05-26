/*
   The MIT License

   Copyright (c) 2008 Bryan Bartow

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in
   all copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
   THE SOFTWARE.
 */
package org.riateam.utils
{
	/**
	 * Auto sizing List.  Height adjusts to fit contents, even when rows are of variable height.
	 * @author Bryan Bartow | bryanbartow@gmail.com | http://www.bryanbartow.com
	 * <p>Code mostly taken from http://workdayui.wordpress.com/2008/05/09/autosizingadvanceddatagrid-that-fixes-the-variablerowheight-issues-with-mxcontrolsadvanceddatagrid/</p>
	 * <p>This may or may not work.  Don't blame me if it destroys your computer or your life.</p>
	 */
	import flash.geom.Point;
	
	import mx.controls.Tree;
	import mx.core.ScrollPolicy;

	public class AutoSizingTree extends Tree
	{
		public function AutoSizingTree()
		{
			super();
			this.defaultRowCount=0;
			this.verticalScrollPolicy = ScrollPolicy.OFF;
		}

		protected var contentHeight:int=0;

		/**
		 * Adjusts the height of the grid until it either runs out of the rows to draw or reaches maxHeight (if maxHeight has been set)
		 */
		protected function getMeasuredHeight(maxHeight:int):Number
		{
			/*if the collection has only one row, we ignore the maxHeight by setting it back to its default
			   One row cannot scroll, setting a max on that for one very large row (that is more than maxHeight pixels long) will force the
			 row to be cliped */
			var count:int=(collection) ? collection.length : 0;
			if (count < 0)
				count=0;
			if (collection && count == 1)
				maxHeight=DEFAULT_MAX_HEIGHT;
			if (contentHeight >= maxHeight)
				return maxHeight;
			var hh:int=0;
			if (!rowInfo || rowInfo.length == 0)
			{
				if (collection)
				{
					contentHeight=Math.min(maxHeight, count * 20);
				}
				else
					contentHeight=0;
				return contentHeight;
			}
			/* keep on increasing the height until either we run out of rows to draw or maxHeight is reached */
			var len:int=Math.min(rowInfo.length, count);
			for (var i:int=0; i < len; i++)
			{
				/* if ( rowInfo[i] && ListRowInfo(rowInfo[i]).uid ) {
				   hh += ListRowInfo(rowInfo[i]).height;
				 } */
				if (rowInfo[i])
				{
					hh+=rowInfo[i].height;
				}
			}

			/* if hh is less than maxHeight and we still have rows to show, increase the height */
			if (hh < maxHeight && rowInfo.length < count)
			{
				/* if we have already drawn all the rows without hitting the maxHeight, we are good to go */
				hh=Math.min(maxHeight, hh + (count - rowInfo.length) * 20);
			}
			contentHeight=Math.min(maxHeight, hh);
			return contentHeight;
		}

		protected function measureHeight():void
		{
			var buffer:int=((this.horizontalScrollBar != null) ? this.horizontalScrollBar.height : 0);
			//var maxContentHeight:int = maxHeight - (headerHeight + buffer);
			var maxContentHeight:int=maxHeight - buffer;
			//var listContentHeight:int = this.headerHeight + buffer + getMeasuredHeight(maxContentHeight);
			var listContentHeight:int=buffer + getMeasuredHeight(maxContentHeight);
			var hh:int=listContentHeight + 2;
			if (hh == this.height)
				return;
			listContent.height=listContentHeight;
			this.height=hh;
			if (height >= maxHeight)
			{
				this.verticalScrollPolicy="auto";
			}
			else
				this.verticalScrollPolicy="off";
		}

		/**
		 * Override of the corresponding method in mx.controls.AdvancedDataGrid.  After drawing the rows, it calls measureHeight to figure out if
		 * the height of the grid still needs to be adjusted
		 */
		protected override function makeRowsAndColumns(left:Number, top:Number, right:Number, bottom:Number, firstCol:int, firstRow:int, byCount:Boolean=false, rowsNeeded:uint=0.0):Point
		{
			var p:Point=super.makeRowsAndColumns(left, top, right, bottom, firstCol, firstRow, byCount, rowsNeeded);
			measureHeight();
			return p;
		}

		/**
		 * displayWidth is a private variable in mx.controls.AdvancedDataGridBaseEx.  We need to create it here so that we can
		 * use it
		 */
		protected var displayWidth:Number;

		/**
		 * We need to override the updateDisplayList so that we can set the displayWidth.
		 * See the displayWidth variable in mx.controls.AdvancedDataGridBaseEx
		 */
		protected override function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void
		{
			if (displayWidth != unscaledWidth - viewMetrics.right - viewMetrics.left)
			{
				displayWidth=unscaledWidth - viewMetrics.right - viewMetrics.left;
			}
			super.updateDisplayList(unscaledWidth, unscaledHeight);
		}
	}
}