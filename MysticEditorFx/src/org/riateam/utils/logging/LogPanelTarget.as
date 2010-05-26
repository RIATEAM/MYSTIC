// be sure to use the mx_internal namespace (and import it)
package org.riateam.utils.logging
{
    import mx.controls.TextArea;
    import mx.core.mx_internal;
    import mx.logging.targets.LineFormattedTarget;
    use namespace mx_internal;

    public class LogPanelTarget extends LineFormattedTarget
    {
        private var console:TextArea;

        public function LogPanelTarget(console : TextArea)
        {
            super();
            this.console = console;
        }

        override mx_internal function internalLog(message : String):void
        {
            this.console.text += message + "\n";
        }
    }
}