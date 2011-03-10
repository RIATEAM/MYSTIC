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
          Home
        </title>
        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
      </head>
      <body onscroll="ScrollLeftToo()">
        <form name="scrollpos">
          <input type="Hidden" name="y" value="0" />
        </form>
        <div id="divsize">
          <div class="images">
            <img src="{Page/Theme/@Path|text()}\logo.jpg" />
          </div>
          <br />
          <br />
          <div class="title">
            <xsl:value-of select="Page/TitoloContent/text()" disable-output-escaping="yes"/>
          </div>
          <br />
          <br />
          <br />
          <br />
          <div class="text">
            <xsl:value-of select="Page/Corpo/text()" disable-output-escaping="yes"/>
          </div>
          <br />
          <br />
          <br />
          <div class="node">
            DATA DI CREAZIONE:<xsl:text>   </xsl:text><xsl:value-of select="Page/DataCreazione/text()" disable-output-escaping="yes"/>
          </div>
          <br />
          <div class="node">
            DATA DI ULTIMA MODIFICA:<xsl:text>   </xsl:text><xsl:value-of select="Page/DataModifica/text()" disable-output-escaping="yes"/>
          </div>
          <br />
          <xsl:if test="Page/Content/@State|text() = '1'">
            <xsl:choose>
              <xsl:when test="Page/Content/@PublishActive|text() = '1'" >
                <div class="node">
                  IN PUBBLICAZIONE IL<xsl:text>   </xsl:text><xsl:value-of select="Page/Content/@DatePublish|text()" disable-output-escaping="yes"/>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <div class="node">
                  DA PUBBLICARE
                </div>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
          <div class="text">

            <xsl:if test="Page/WIDGETS/WIDGET/@Titolo|text() != ''" >
              <br/>
              <br/>
              <br/>
            </xsl:if>
          </div>

          <xsl:apply-templates select="Page/WIDGETS/WIDGET" mode="child" />
        </div>
      </body>
    </html>

  </xsl:template>

  <xsl:template match="*" mode="child">
    <center>
      <div class="nodearancione">
        <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes"/>
      </div>
      <br/>
      <xsl:apply-templates select="WIDGETELEMENT" mode="childs" />
    </center>
  </xsl:template>
  <xsl:template match="*" mode="childs">
    <div>
      <a href="{@Href|text()}" target="{@Target|text()}">
        <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
      </a>
    </div>
  </xsl:template>
</xsl:stylesheet>
