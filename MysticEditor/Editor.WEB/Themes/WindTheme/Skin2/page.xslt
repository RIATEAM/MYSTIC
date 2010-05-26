<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" encoding="UTF-8"
            indent="no"/>

  <xsl:template match="/">
    <html>
      <head>

        <title>
          <xsl:value-of select="Page/Titolo/text()" disable-output-escaping="yes"/>
        </title>
        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
      </head>
      <body>


        <xsl:value-of select="Page/Titolo/text()" disable-output-escaping="yes"/>
        <br />

        <xsl:value-of select="Page/Corpo/text()" disable-output-escaping="yes"/>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>
