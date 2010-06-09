<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" encoding="UTF-8"
            indent="no"/>

  <xsl:template match="/">
    <html>
      <head>
        <link rel="stylesheet" href="{Page/Theme/@Path|text()}\styles\style.css" />
        <title>
          Home
        </title>
        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
      </head>
      <body>
        <div class="title">
          <xsl:value-of select="Page/Titolo/text()" disable-output-escaping="yes"/>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <div class="node">
          DATA DI CREAZIONE:  <xsl:value-of select="Page/DataCreazione/text()" disable-output-escaping="yes"/>
        </div>
        <br />
        <div class="node">
          DATA DI ULTIMA MODIFICA:  <xsl:value-of select="Page/DataModifica/text()" disable-output-escaping="yes"/>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>
