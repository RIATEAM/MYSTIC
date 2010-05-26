<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" encoding="UTF-8"
            indent="no"/>

  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>

        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
        <title>Menu</title>

        <link rel="stylesheet" href="{Menu/Theme/@Path|text()}\js\jquery.treeview.css" />
        <link rel="stylesheet" href="{Menu/Theme/@Path|text()}\js\red-treeview.css" />
        <link rel="stylesheet" href="{Menu/Theme/@Path|text()}\styles\screen.css" />

        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js">
          // commento
        </script>
        <script src="{Menu/Theme/@Path|text()}\js\lib\jquery.cookie.js" type="text/javascript">
          // commento
        </script>
        <script src="{Menu/Theme/@Path|text()}\js\jquery.treeview.js" type="text/javascript">
          // commento
        </script>

        <script type="text/javascript">
          $(document).ready(function(){
          $("#browser").treeview({
          control:"#sidetreecontrol"
          });
          });
        </script>
      </head>
      <body>

        <div id="sidetreecontrol">
          <a href="?#">Collapse All</a> | <a href="?#">Expand All</a>
        </div>
        <div id="main">

          <ul id="browser" class="filetree treeview-famfamfam">
            <li>
              <span class="folder">Home</span>
              <ul>
                <xsl:apply-templates select="Menu/Item" mode ="items" />
              </ul>
            </li>
          </ul>
        </div>

      </body>
    </html>

  </xsl:template>


  <xsl:template match="*" mode="items">
    <li>
      <xsl:choose>
        <xsl:when test="@State|text() = '1'">
          <span class="file">
            <a href="{@Href|text()}" target="{@Target|text()}">
              <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
            </a>
          </span>
        </xsl:when>
        <xsl:otherwise>
          <span class="fileno">
            <a class="stroke" href="{@Href|text()}" target="{@Target|text()}">
              <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
            </a>
          </span>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:if test="Item/@Id|text() != ''" >
        <ul>
          <xsl:apply-templates select="Item" mode ="items" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
</xsl:stylesheet>
