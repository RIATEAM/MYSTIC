<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	exclude-result-prefixes="msxsl">
  <xsl:output method="html" encoding="UTF-8" indent="no" />
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />
        <title>Menu</title>
        <link rel="stylesheet" href="{Menu/Theme/@Path|text()}\js\jquery.treeview.css" />
        <link rel="stylesheet" href="{Menu/Theme/@Path|text()}\styles\screen.css" />
        <script type="text/javascript" src="{Menu/Theme/@Path|text()}\js\jquery.min.js">
          // commento
        </script>
        <script src="{Menu/Theme/@Path|text()}\js\lib\jquery.cookie.js" type="text/javascript">
          // commento
        </script>
        <script src="{Menu/Theme/@Path|text()}\js\jquery.treeview.js" type="text/javascript">
          // commento
        </script>
        <script src="{Menu/Theme/@Path|text()}\js\custom.js" type="text/javascript">
          // commento
        </script>
       <!-- <script src="{Menu/Theme/@Path|text()}\js\frameset.js" type="text/javascript">
          // commento
        </script>-->
        <script type="text/javascript">
          $(document).ready(function(){
          $("#browser").treeview({
          control:"#sidetreecontrol"
          });
          
          $("#browser2").treeview();
          <!--setTimeout('setSize()', 100);-->
          });
        </script>
      </head>
      <body>
        <form name="scrollpos">
          <input type="Hidden" name="menuheight" value="0" />
          <input type="Hidden" id="currentSelected" name="currentSelected" value="0" />
          <input type="Hidden" id="oldBack" name="oldBack" value="0" />
          <input type="Hidden" id="oldFont" name="oldFont" value="0" />
        </form>
        <div id="sidetreecontrol">
          <a href="?#"></a>
          <a onmouseover="expOnMouseOver(this)" onmouseout="expOnMouseOut(this)" href="?#">Espandi Tutto</a>
        </div>
        <div id="main">
          <ul id="browser" class="filetree treeview-famfamfam">
            <xsl:apply-templates select="Menu/Item" mode="items" />
          </ul>
          <br/>
          <br/>
          <ul id="browser2" class="filetree treeview-famfamfam">
            <xsl:apply-templates select="Menu/WIDGETS" mode="widgets" />
         </ul>
        </div>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="*" mode="widgets">
    <xsl:if test="WIDGET/@Id|text() != ''">
      <ul>
        <xsl:apply-templates select="WIDGET" mode="widget" />
      </ul>
    </xsl:if>
  </xsl:template>
  <xsl:template match="*" mode="widget">
    <li>
      <span class="file">
          <span class="arancione"><xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" /></span>
      </span>
      <xsl:if test="WIDGETELEMENT/@Id|text() != ''">
        <ul>
          <xsl:apply-templates select="WIDGETELEMENT" mode="widgetelement" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="*" mode="widgetelement">
    <li>
      <span class="file">
        <a onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
          href="{@Href|text()}" target="{@Target|text()}">
          <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
        </a>
      </span>
      <xsl:if test="WIDGETELEMENT/@Id|text() != ''">
        <ul>
          <xsl:apply-templates select="WIDGETELEMENT" mode="widgetelement" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="*" mode="items">
    <li>
      <xsl:if test="@State|text() = '1'">
        <span class="file">
          <xsl:choose>
            <xsl:when test="parent::node()/@Titolo|text() = 'HOME' or name(parent::node()) = 'Menu'">
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                class="arancione" href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:when>
            <xsl:otherwise>
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </xsl:if>
      <xsl:if test="@State|text() = '2'">
        <span class="fileno">
          <xsl:choose>
            <xsl:when test="parent::node()/@Titolo|text() = 'HOME' or name(parent::node()) = 'Menu'">
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                class="stroke arancione" href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:when>
            <xsl:otherwise>
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                class="stroke" href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </xsl:if>
      <xsl:if test="@State|text() = '3'">
        <span class="file">
          <xsl:choose>
            <xsl:when test="parent::node()/@Titolo|text() = 'HOME' or name(parent::node()) = 'Menu'">
                <span class="arancione"><xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" /></span>
            </xsl:when>
            <xsl:otherwise>
                <span><xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" /></span>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </xsl:if>
      <xsl:if test="@State|text() = '4'">
        <span class="fileagg">
          <xsl:choose>
            <xsl:when test="parent::node()/@Titolo|text() = 'HOME' or name(parent::node()) = 'Menu'">
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                class="arancione" href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:when>
            <xsl:otherwise>
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </xsl:if>
      <xsl:if test="@State|text() = '5'">
        <span class="file">
          <xsl:choose>
            <xsl:when test="parent::node()/@Titolo|text() = 'HOME' or name(parent::node()) = 'Menu'">
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                class="arancione" href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:when>
            <xsl:otherwise>
              <a id="{@Id|text()}" onmousedown="onMouseDown(this)" onmouseover="onMouseOver(this)" onmouseout="onMouseOut(this)"
                href="{@Href|text()}" target="{@Target|text()}">
                <xsl:value-of select="@Titolo|text()" disable-output-escaping="yes" />
              </a>
            </xsl:otherwise>
          </xsl:choose>
        </span>
      </xsl:if>
      <xsl:if test="Item/@Id|text() != ''">
        <ul>
          <xsl:apply-templates select="Item" mode="items" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
</xsl:stylesheet>
