package org.riateam.utils.managers.mappable
{
	import flash.events.*;
	import flash.xml.XMLDocument;
	import flash.xml.XMLNode;

	import mx.controls.Alert;
	import mx.core.Application;
	import mx.rpc.Fault;
	import mx.rpc.events.*;
	import mx.rpc.http.HTTPService;
	import mx.rpc.xml.*;
	import mx.utils.XMLUtil;

	import org.riateam.domain.MysticMapSchemaDAO;
	import org.riateam.utils.managers.DebugManager;
	import org.spicefactory.lib.logging.LogContext;

	/**
	 * Event raised by superclass HTTPService are Excluded
	 */
	[Exclude(name="result", kind="event")]
	/**
	 * Event raised by superclass HTTPService are Excluded
	 */
	[Exclude(name="fault", kind="event")]

	/**
	 * DynamicEvent raised by result
	 */
	[Event(name="mappableResult", type="org.riateam.utils.managers.mappable.MappableHTTPResultEvent")]
	/**
	 * DynamicEvent raised by fault
	 */
	[Event(name="mappableFault", type="org.riateam.utils.managers.mappable.MappableHTTPFaultEvent")]

	/**
	 *
	 *
	 */
	public class MappableHTTPService extends HTTPService
	{
		//TODO: describe getter/setter methods instead of public var

		private var schemaLoader:SchemaLoader;
		private var schemaManager:SchemaManager;
		private var schema:Schema;
		private var _schemaAlreadyLoaded:Boolean=false;
		public var schemaTypeRegistry:SchemaTypeRegistry;
		public var schemaMapping:Array;
		public var rootUriDefinition:String;

		private var _xsdUrl:String; // = "schema.xsd"

		public function get XsdUrl():String
		{
			return _xsdUrl;
		}

		public function set XsdUrl(value:String):void
		{
			_xsdUrl=value;
			if (!_schemaAlreadyLoaded)
				this.loadXMLSchema();
		}



		/**
		 *ctor
		 *
		 */
		public function MappableHTTPService()
		{
			super();
			this.resultFormat=RESULT_FORMAT_E4X;

			this.addEventListener(ResultEvent.RESULT, onResult);
			this.addEventListener(FaultEvent.FAULT, onFault);
			this.contentType="text/xml"; //VERIFICARE "application/xml";


		}


		private function onResult(event:ResultEvent):void
		{
			var xmlData:XML=XML(event.result);
			// var result:* = decodeXML(xmlData);
			/*  var newEvent:ResultEvent = ResultEvent.createEvent(result) */
			if (xmlData.children().length() == 0)
			{
				Alert.show("xmlData.length() ==0 onResult handler", "Errore");
			}

			var resultObj:*=decodeXML(xmlData);
			var newEvent:MappableHTTPResultEvent=new MappableHTTPResultEvent(resultObj);
			dispatchEvent(newEvent);
		}

		private function onFault(event:FaultEvent):void
		{

			serviceOnFault(event.fault);
			//Alert.show(errMsg ,"Errore");
		}

		/**
		 * Creates an instance of SchemaLoader and loads the xsd file.
		 */
		public function loadXMLSchema():void
		{
			setLog("loadXMLSchema()");
			schemaManager=new SchemaManager();
			schemaLoader=new SchemaLoader();
			schemaLoader.addEventListener(SchemaLoadEvent.LOAD, schemaLoader_loadHandler);
			schemaLoader.addEventListener(XMLLoadEvent.LOAD, schemaLoader_xmlLoadHandler);
			schemaLoader.addEventListener(FaultEvent.FAULT, schemaLoader_faultHandler);

			schemaLoader.load(XsdUrl);
		}

		/**
		 * Adds a schema to the SchemaManager and registers any ActionScript classes to specific schema types
		 */
		public function setXMLSchema(value:Schema):void
		{
			schema=value;

			//Add the loaded schema to the SchemaManager
			schemaManager.addSchema(schema);

			//Map the XSD type "example" to the ActionScript class Diagram
			schemaTypeRegistry=SchemaTypeRegistry.getInstance();
			/* schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "MysticMapSchemaDAO"), MysticMapSchemaDAO);
			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "MysticServicesDAO"), MysticServicesDAO);

			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "MysticServicesDAO"), MysticServicesDAO);
			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "MysticErrorDAO"), MysticErrorDAO);

			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "ImageMapDAO"), ImageMapDAO);
			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "PickMapActionDAO"), PickMapActionDAO);
			   schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "PickMapDAO"), PickMapDAO);
			 schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, "MysticMapPropertiesDAO"), MysticMapPropertiesDAO); */
			for each (var mapping:MappableSchemaObject in schemaMapping)
			{
				schemaTypeRegistry.registerClass(new QName(schema.targetNamespace.uri, mapping.uriLocalName), mapping.definition);
			}
		}


		/**
		 * Decodes XML into ActionScript objects using the schema definitions within SchemaManag	er
		 */
		public function decodeXML(xml:XML):*
		{
			setLog("decodeXML()");
			var qName:QName;
			var xmlDecoder:XMLDecoder;
			var result:*;
			var root:QName=xml.name();


			DebugManager.instance.setInspectorPopUp(xml.toString(), "Action = GET |XML received from the servlet", Application.application.mainView);
			try
			{
				if (schema)
					qName=new QName(schema.targetNamespace.uri, rootUriDefinition);
				/* qName=new QName(schema.targetNamespace.uri, "MysticMapSchema"); */


				xmlDecoder=new XMLDecoder();
				xmlDecoder.schemaManager=schemaManager;
				xmlDecoder.typeRegistry=schemaTypeRegistry;

				result=xmlDecoder.decode(xml, qName);
			}
			catch (err:Error)
			{
				Alert.show(err.message, " Errore in decodeXML() ");

			}
			return result;
		}

		/**
		 *
		 *
		 */
		public function encodeToXML(results:MysticMapSchemaDAO):XMLDocument
		{
			setLog("encodeToXML()");
			var qName:QName;
			var xmlEncoder:XMLEncoder;
			var xmlList:XMLList;
			var o:Object;
			var xmlDoc:XMLDocument;
			try
			{
				if (schema)
				{

					/* qName=new QName(schema.targetNamespace.uri, "MysticMapSchema"); */
					qName=new QName(schema.targetNamespace.uri, rootUriDefinition);
					/* qName = new QName("", "ResultsDTO"); */

					xmlEncoder=new XMLEncoder();
					xmlEncoder.schemaManager=schemaManager;
					xmlEncoder.strictNillability=true;

					//var progetto:TypePDLProgetto=(results.PDLPianoDiLavoro.PDLProgettazione[0]) as TypePDLProgetto;
					xmlList=xmlEncoder.encode(results, qName);


					xmlDoc=XMLUtil.createXMLDocument(xmlList.toXMLString());
					xmlDoc.xmlDecl="<?xml version='1.0' encoding='utf-8'?>\n";
					trace(xmlList.toXMLString());

				}
				/* var xmlDoc:* = objectToXML(results, qName); */
			}
			catch (err:Error)
			{
				var str:String=err.message;
				str+="\n" + err.getStackTrace()
				Alert.show(str, "errore encoding xml");
					//LogContext.getLogger(this).error("qname nullo!");
			}
			return xmlDoc;
		}

		private function objectToXML(obj:Object, qname:QName=null):XML
		{
			if (qname == null)
				qname=new QName("root");
			var xmlDocument:XMLDocument=new XMLDocument();
			var simpleXMLEncoder:SimpleXMLEncoder=new SimpleXMLEncoder(xmlDocument);
			var xmlNode:XMLNode=simpleXMLEncoder.encodeValue(obj, qname, xmlDocument);
			xmlDocument.xmlDecl="<?xml version='1.0' encoding='utf-8'?>\n";
			var xml:XML=new XML(xmlDocument.toString());
			// trace(xml.toXMLString());
			return xml;
		}

		/**
		 * Dispatched once SchemaLoader has completed loading the entire schema
		 */
		private function schemaLoader_loadHandler(event:SchemaLoadEvent):void
		{
			setLog("schemaLoader_loadHandler " + event.schema);
			setXMLSchema(event.schema);
			_schemaAlreadyLoaded=true;
		}

		/**
		 * Dispatched each time SchemaLoader loads a schema
		 * This may occur multiple times if a schema contains any imported/included schemas
		 */
		private function schemaLoader_xmlLoadHandler(event:XMLLoadEvent):void
		{
			setLog("schemaLoader_xmlLoadHandler " + event.location);
		}

		private function xmlService_resultHandler(event:ResultEvent):void
		{
			setLog("xmlService_resultHandler");
			var resultObj:*=decodeXML(event.result as XML);
			var resultEvent:MappableHTTPResultEvent=new MappableHTTPResultEvent(resultObj);

			dispatchEvent(resultEvent);
		}

		private function schemaLoader_faultHandler(event:FaultEvent):void
		{
			setLog("schemaLoader_faultHandler " + event);
			serviceOnFault(event.fault);
		}

		private function serviceOnFault(fault:Fault):void
		{
			var newEvent:MappableHTTPFaultEvent=new MappableHTTPFaultEvent(fault);

			dispatchEvent(newEvent);
		}

		/**
		 * Funzione di logging
		 * TODO: implementare logging!
		 * @param message
		 *
		 */
		public function setLog(message:String):void
		{
			/* if (!Application.application.DEBUG)
			   Alert.show(message);
			 else */
			LogContext.getLogger(MappableHTTPService).debug(message);

		}
	}




}