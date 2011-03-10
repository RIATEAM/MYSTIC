<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" encoding="UTF-8" doctype-system="http://www.w3.org/TR/html4/strict.dtd" doctype-public="-//W3C//DTD HTML 4.01//EN" indent="no"/>

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
        <script src="{Page/Theme/@Path|text()}\js\custom.js" type="text/javascript">
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

        <div id="menuWrapper" class="box">
          <div id="headtitle">
            <a href="#item1">
              <xsl:value-of select="Page/WIDGETS/WIDGET/@Titolo|text()" disable-output-escaping="yes"/>
            </a>
          </div>
          <ul id="cssdropdownmenu">
            <xsl:apply-templates select="Page/WIDGETS/WIDGET/WIDGETELEMENT" mode="widgets" />
          </ul>
        </div>
        <div id="divsize">
          <div class="title">
            <xsl:value-of select="Page/TitoloContent/text()" disable-output-escaping="yes"/>
          </div>
          <div class="node">

            <div class="node" style="width:85%;">
              <xsl:value-of select="translate(Page/Titolo/text(), 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" disable-output-escaping="yes"/>
            </div>


          </div>
          <br />
          <br />
          <br />

          <div class="text">
            <xsl:value-of select="Page/Corpo/text()" disable-output-escaping="yes"/>

            <xsl:if test="Page/Childs/Child/@Href|text() != ''" >
              <br/>
              <br/>
              <br/>
              More:
            </xsl:if>
          </div>

          <xsl:apply-templates select="Page/Childs/Child" mode="childs" />
        </div>

      </body>
    </html>

  </xsl:template>

  <xsl:template match="*" mode="widgets">
    <li>
      <a href="{@Href|text()}" target="{@Target|text()}">
        <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
      </a>
    </li>
  </xsl:template>

  <xsl:template match="*" mode="childs">
    <xsl:if test="@State|text() = '1'">
      <div class="file normal">
        <a href="{@Href|text()}" target="{@Target|text()}" onclick="changeMenu('{@Id|text()}')">
          <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
        </a>
      </div>
    </xsl:if>
    <xsl:if test="@State|text() = '2'">
      <div class="fileno normal">
        <a class="stroke" href="{@Href|text()}" target="{@Target|text()}" onclick="changeMenu('{@Id|text()}')">
          <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
        </a>
      </div>
    </xsl:if>
    <xsl:if test="@State|text() = '3'">
      <div class="file normal">
        <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
      </div>
    </xsl:if>
    <xsl:if test="@State|text() = '4'">
      <div class="file normal">
        <a href="{@Href|text()}" target="{@Target|text()}" onclick="changeMenu('{@Id|text()}')">
          <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
        </a>
      </div>
    </xsl:if>
    <xsl:if test="@State|text() = '5'">
      <div class="file normal">
        <a href="{@Href|text()}" target="{@Target|text()}" onclick="changeMenu('{@Id|text()}')">
          <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
        </a>
      </div>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
