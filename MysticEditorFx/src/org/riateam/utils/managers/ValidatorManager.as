package org.riateam.utils.managers
{
	import flash.display.DisplayObject;
	import flash.events.EventDispatcher;
	import flash.events.IEventDispatcher;
	import mx.events.ValidationResultEvent;
	import mx.validators.Validator;

	/**
	 *
	 * @author gonte
	 *
	 */
	[Bindable]
	public class ValidatorManager extends EventDispatcher
	{
		public var formIsValid:Boolean=false;
		public var validators:Array;
		public var focusedFormControl:DisplayObject;


		public function ValidatorManager(target:IEventDispatcher=null)
		{
			super(target);
		}

		public function validateForm():void
		{
			//focusedFormControl=event.target as DisplayObject;
			formIsValid=true;
			for each (var validator:Validator in validators)
				validate(validator);
		}


		private function validate(validator:Validator):Boolean
		{
			var validatorSource:DisplayObject=validator.source as DisplayObject;
			var supressEvents:Boolean=true; //validatorSource != focusedFormControl;

			var event:ValidationResultEvent=validator.validate(null, supressEvents)
			var currentControlIsValid:Boolean=event.type == ValidationResultEvent.VALID;
			formIsValid=formIsValid && currentControlIsValid;
			if (!formIsValid)
				trace(validatorSource);
			return currentControlIsValid;
		}
	/* 	public function validateForm(event:Event):void
	   {
	   focusedFormControl=event.target as DisplayObject;
	   formIsValid=true;
	   for each (var validator:Validator in validators)
	   validate(validator);
	   }


	   private function validate(validator:Validator):Boolean
	   {
	   var validatorSource:DisplayObject=validator.source as DisplayObject;
	   var supressEvents:Boolean=validatorSource != focusedFormControl;
	   focusedFormControl.transform.colorTransform.redOffset = 100;
	   var event:ValidationResultEvent=validator.validate(null, supressEvents)
	   var currentControlIsValid:Boolean=event.type == ValidationResultEvent.VALID;
	   formIsValid=formIsValid && currentControlIsValid;
	   return currentControlIsValid;
	   }
	 */

	/* public function addValidator(validator:Validator):void
	   {
	   validatorList.push(validator);

	 } */

	}
}


