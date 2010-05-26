package Welsy3.UI.utils.Base
{
	import Welsy3.BE.util.Persistent;

	import flash.events.FocusEvent;

	import mx.collections.ArrayCollection;
	import mx.collections.Sort;
	import mx.collections.SortField;
	import mx.controls.ComboBox;
	import mx.events.ListEvent;
	import mx.validators.NumberValidator;
	import mx.validators.Validator;

	/**
	 * Combobox con gestione PromptText e set selectedIndexFromValue
	 *
	 *
	 */
	public class PersistentComboBox extends ComboBox
	{
		private var DataField_:String="";
		private var ValueField_:String="data";
		public var promptText:String=null;
		private var selectedIndexFromValue_:String="";
		private var selectedIndexFromData_:Number;
		private var ObjPersistente_:Persistent;
		private var ObjValidator_:Validator=null;
		private var ValueRequired_:Boolean=false;
		private var _ValueRequiredChanged:Boolean=false;

		//CTOR
		public function PersistentComboBox()
		{
			super();
			this.labelField="";
			this.rowCount=10;
			this.percentWidth=100;
			this.addEventListener(ListEvent.CHANGE, ChangeHandler);
			this.addEventListener(FocusEvent.FOCUS_OUT, FocusOutHandler);
		}

		public function get DataField():String
		{
			return DataField_;
		}

		public function set DataField(value:String):void
		{
			DataField_=value;
			invalidateProperties();
		}

		public function get ValueField():String
		{
			return ValueField_;
		}

		public function set ValueField(value:String):void
		{
			ValueField_=value;
			invalidateProperties();
		}

		//  modificato x consentire uso classe selectable prompt senza indicare prompt
		override public function set dataProvider(value:Object):void
		{
			//var dp: ArrayCollection=sortDataProvider(value);
			if (promptText != null)
			{

				super.dataProvider=new PromptArrayCollection([promptText], value as ArrayCollection);
			}
			else
			{
				super.dataProvider=value;
			}
			invalidateProperties();

		}

		/**
		 * Check if there is prompText inside the dataprovider
		 * @return
		 *
		 */
		public function get HasPromptText():Boolean
		{
			return this.dataProvider is PromptArrayCollection;
		}

// non usato
		private function sortDataProvider(source:ArrayCollection):ArrayCollection
		{

			var ac:ArrayCollection=ArrayCollection(source);
			var s:Sort=new Sort();

			if (this.labelField != "label")
			{
				var field:SortField=new SortField();
				field.name=this.labelField;
				s.fields=[field];
				ac.sort=s;
			}
			ac.refresh();
			return ac;

		}

		override public function get dataProvider():Object
		{
			return super.dataProvider;
		}

		public function set selectedIndexFromValue(value:String):void
		{
			var retval:Number=0;
			var cmbItem:Object;
			var field:String=this.labelField;
			var datap:ArrayCollection=ArrayCollection(this.dataProvider);

			//index == 0 corrisponde al prompt
			var start:int=0;
			if (!HasPromptText)
				start=1;
			for (var i:int=start; i < datap.length; i++)
			{
				cmbItem=datap[i];

				if (cmbItem[field] == value)
				{
					retval=i;
					break;
				}
			}

			super.selectedIndex=retval;
			invalidateProperties();

		}

		public function get selectedIndexFromValue():String
		{
			return this.selectedIndexFromValue_;
		}

		///   aggiunta funzione x ricerca selected index anche value da ricercare in DATA 
		public function set selectedIndexFromData(val:Number):void
		{
			var dataVal:String=String(val);
			var retval:Number=0;
			var cmbItem:Object;
			var field:String=this.ValueField;
			var datap:ArrayCollection=ArrayCollection(this.dataProvider);

			//index starts from 1, value0 is PrompText
			for (var i:int=1; i < datap.length; i++)
			{
				cmbItem=datap[i];
				//verify value property
				if (!cmbItem.hasOwnProperty(field))
					break;
				var cmbData:String=String(cmbItem[field])

				if (cmbData == dataVal)
				{
					retval=i;
					break;

				}
			}
			super.selectedIndex=retval;
			invalidateProperties();

		}

		public function get selectedIndexFromData():Number
		{
			return this.selectedIndexFromData_;
		}

		public function get ObjPersistente():Persistent
		{
			return ObjPersistente_;
		}

		public function set ObjPersistente(value:Persistent):void
		{
			if (value)
			{
				ObjPersistente_=Persistent(value);
				invalidateProperties();
			}
		}

		public function get ObjValidator():Validator
		{
			return ObjValidator_;
		}

		public function set ObjValidator(value:Validator):void
		{
			if (value)
			{
				ObjValidator_=Validator(value);
				invalidateProperties();
			}
		}

		public function get ValueRequired():Boolean
		{
			return ValueRequired_;
		}

		public function set ValueRequired(value:Boolean):void
		{
			ValueRequired_=Boolean(value);
			_ValueRequiredChanged=true;
			invalidateProperties();
		}

		override public function set enabled(value:Boolean):void
		{
			super.enabled=value;
			invalidateProperties();
		}

		/** handlers*/
		private function ChangeHandler(evt:ListEvent):void
		{
			if (ObjPersistente != null)
			{
				ObjPersistente.Dirty=true;
			}
		}

		private function FocusOutHandler(evt:FocusEvent):void
		{
			if (this.selectedIndex > 0)
			{
				if (DataField_)
					if (ObjPersistente)
						if (ObjPersistente.hasOwnProperty(DataField_))
							if (this.selectedItem.hasOwnProperty(ValueField_))
								ObjPersistente[DataField_]=this.selectedItem[ValueField_];

			}

			if (ValueRequired && ObjValidator != null)
			{
				ObjValidator.validate();
			}
		}

		private function initValidator():NumberValidator
		{
			var validator:NumberValidator=new NumberValidator();
			validator.source=this;
			validator.property="selectedIndex";
			validator.minValue=1;
			validator.lowerThanMinError="Selezionare un valore.";
			return validator;
		}


		/**
		 *
		 *
		 */
		override protected function commitProperties():void
		{
			super.commitProperties();

			if (ObjPersistente_ != null)
			{
				//verifica validators
				var indexValidator:int;
				if (_ValueRequiredChanged)
				{
					if (ValueRequired)
					{
						if (ObjValidator == null)
							ObjValidator=initValidator();

						indexValidator=ObjPersistente_.Validators.getItemIndex(ObjValidator);

						//aggiungo ObjValidator_ se non è già presente nella lista di validators dell'ObjPersistente_
						if (indexValidator == -1)
						{
							ObjPersistente_.Validators.addItem(ObjValidator);
						}
					}

					else
					{
						indexValidator=ObjPersistente_.Validators.getItemIndex(ObjValidator);

						//se !ValueRequired elimino ObjValidator_ se  è  presente nella lista di validators dell'ObjPersistente_
						if (indexValidator > -1)
							ObjPersistente_.Validators.removeItemAt(indexValidator);
					}
					_ValueRequiredChanged=false;
				}
				//disabilitazione validator
				if (ObjValidator)
					ObjValidator.enabled=this.enabled;

					//assegnazione valore gestita tramite focusOut event
			}
		}

	}
}

