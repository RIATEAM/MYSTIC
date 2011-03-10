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
        <script type="text/javascript" src="{Page/Theme/@Path|text()}\js\jquery.min.js">
          // commento
        </script>
        <script src="{Page/Theme/@Path|text()}\js\frameset.js" type="text/javascript">
          // commento
        </script>
        <title>
          <xsl:value-of select="Page/Titolo/text()" disable-output-escaping="yes"/>
        </title>
        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
      </head>
      <body onscroll="ScrollLeftToo()">

        <form name="scrollpos">
          <input type="Hidden" name="y" value="0" />
        </form>
        <div id="divsize">
          <div class="title">
            <xsl:value-of select="Page/TitoloContent/text()" disable-output-escaping="yes"/>
          </div>
          <div class="node">
            <xsl:value-of select="translate(Page/Titolo/text(), 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" disable-output-escaping="yes"/>
          </div>
          <br />
          <br />
          <br />

          <div class="text">
            <xsl:value-of select="Page/Corpo/text()" disable-output-escaping="yes"/>
          </div>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>
