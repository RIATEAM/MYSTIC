package org.riateam.utils.managers
{
	import flash.events.Event;
	import flash.events.EventDispatcher;

	import mx.collections.ArrayCollection;
	import mx.collections.CursorBookmark;
	import mx.collections.IViewCursor;
	import mx.collections.Sort;
	import mx.collections.SortField;

	[Event(name="selectionChange", type="flash.events.Event")]

	public class CollectionViewManager extends EventDispatcher
	{
		public static const NEXT:String="next";
		public static const BACK:String="back";
		private static var _instance:CollectionViewManager;

		public var cursor:IViewCursor;
		[Bindable]
		public var collectionView:ArrayCollection;
		[Bindable]
		public var currentIndex:int=0;
		[Bindable]
		public var IsFirst:Boolean;
		[Bindable]
		public var IsLast:Boolean;
		public static var SELECTION_CHANGE:String="selectionChange";
		public static var DATAPROVIDER_CHANGE:String="dataProviderChange";

		public function CollectionViewManager()
		{
			this.addEventListener(DATAPROVIDER_CHANGE, function dpChange(event:Event):void
				{
					sortDataProvider();
					//cursor=null;
					cursor=collectionView.createCursor();
					firstCollection();
				});
		}

		[Bindable]
		public var selectedItem:Object;

		public function get SelectedItem():Object
		{
			return selectedItem;
		}

		public function set SelectedItem(value:Object):void
		{
			selectedItem=value;
			selectionChange();
		}

		public function get CurrentIndex():int
		{
			return currentIndex;
		}

		public function set CurrentIndex(value:int):void
		{
			currentIndex=value;
			enableDisableButtons();
		}

		public static function get instance():CollectionViewManager
		{
			if (!_instance)
			{
				_instance=new CollectionViewManager();
			}
			return _instance;
		}

		public function set DataProvider(value:Object):void
		{
			collectionView=(value as ArrayCollection);
			dispatchEvent(new Event(DATAPROVIDER_CHANGE));
		}


		public function setSelectableItems(element:Object):Array
		{
			var retVal:Array;
			/* find(element, cursor); */
			if (cursor.findFirst(element))
			{
				retVal=[cursor.current];
				SelectedItem=cursor.current;
				CurrentIndex=cursor.bookmark.getViewIndex()
			}

			return retVal;
		}

		public function find(item:Object, theCursor:IViewCursor):void
		{
			var counter:int=-1;
			theCursor.seek(CursorBookmark.FIRST);

			while (theCursor.moveNext())
			{
				counter++;
				if (item == theCursor.current)
				{
					SelectedItem=theCursor.current;
					CurrentIndex=counter;
					return;
				}
			}


		}

		public function sortDataProvider():void
		{
			var sort:Sort=new Sort();
			sort.fields=[new SortField("id")];
			collectionView.sort=sort;
			collectionView.refresh();
		}

		public function countLast(theCursor:IViewCursor):int
		{
			var counter:int=0;
			var mark:CursorBookmark=theCursor.bookmark;
			while (theCursor.moveNext())
			{
				counter++;
			}
			theCursor.seek(mark);
			return counter;
		}

		public function nextCollection():void
		{
			if (!cursor.afterLast)
			{
				cursor.moveNext();
				CurrentIndex=cursor.bookmark.getViewIndex()
				SelectedItem=cursor.current;
			}
		}

		public function backCollection():void
		{
			if (!cursor.beforeFirst)
			{
				cursor.movePrevious();
				CurrentIndex=cursor.bookmark.getViewIndex()
				SelectedItem=cursor.current;
			}
		}

		public function firstCollection():void
		{
			cursor.seek(CursorBookmark.FIRST);
			CurrentIndex=cursor.bookmark.getViewIndex();
			SelectedItem=cursor.current;
		}

		public function lastCollection():void
		{
			cursor.seek(CursorBookmark.LAST);
			CurrentIndex=cursor.bookmark.getViewIndex();
			SelectedItem=cursor.current;
		}

		public function navigate(direction:String=NEXT):void
		{
			switch (direction)
			{
				case NEXT:
				{
					nextCollection();
					break;
				}
				case BACK:
				{
					backCollection();
					break;
				}

			}

		}

		public function enableDisableButtons():void
		{
			if (collectionView.length > 0)
			{
				var firstInCollection:Boolean=collectionView.getItemAt(0) == cursor.current;
				IsFirst=firstInCollection;
				var lastItem:Object=collectionView.getItemAt(collectionView.length - 1);
				var lastInCollection:Boolean=lastItem == cursor.current;
				IsLast=lastInCollection;
			}
		}

		private function selectionChange():void
		{
			dispatchEvent(new Event(SELECTION_CHANGE));
		}

	}
}