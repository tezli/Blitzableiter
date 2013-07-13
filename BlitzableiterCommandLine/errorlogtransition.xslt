<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

  <xsl:output method="xml" indent="yes" encoding="UTF-8"/>

  <xsl:template match="/scan">
    <html>
      <head>
        <link rel="stylesheet" href="style.css" type="text/css"/>
        <title>Blitzableiter Rejected Files</title>
      </head>
      <body>
        <h1>Rejected Files</h1>
        <table>
          <tr>
            <td>
              Total : <xsl:value-of select="/scan/@filestotal"/>
            </td>
            <td>
              Rejected : <xsl:value-of select="/scan/@filesrejected"/>
            </td>
            <td>
              Fail Ratio : <xsl:value-of select="/scan/@ratio"/> %
            </td>
          </tr>
        </table>
        <br/>
        <br/>
        <br/>
        <table>
          <xsl:apply-templates select="item">
          </xsl:apply-templates>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="item">
    <tr>
      <td>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="directory"/>
          </xsl:attribute>
          <xsl:value-of select="directory"/>\<xsl:value-of select="filename"/>
        </a>
      </td>
      <td colspan="2">
        <xsl:value-of select="error"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
