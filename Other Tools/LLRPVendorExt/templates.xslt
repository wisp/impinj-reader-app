<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:llrp="http://www.llrp.org/ltk/schema/core/encoding/binary/1.0"
  xmlns:h="http://www.w3.org/1999/xhtml">
  <xsl:output omit-xml-declaration='yes' method='text' indent='yes'/>

  <xsl:template name="DefineDataType" match="field">
    <xsl:choose>
      <xsl:when test="@enumeration and @type!='u8v'">
        ENUM_<xsl:value-of select="@enumeration"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="@type = 'u1'">bool</xsl:when>
          <xsl:when test="@type = 'u1v'">LLRPBitArray</xsl:when>
          <xsl:when test="@type = 'u2'">TwoBits</xsl:when>
          <xsl:when test="@type = 'u8'">byte</xsl:when>
          <xsl:when test="@type = 's8'">sbyte</xsl:when>
          <xsl:when test="@type = 'u8v'">ByteArray</xsl:when>
          <xsl:when test="@type = 'u16'">UInt16</xsl:when>
          <xsl:when test="@type = 'u16v'">UInt16Array</xsl:when>
          <xsl:when test="@type = 's16'">Int16</xsl:when>
          <xsl:when test="@type = 'u32'">UInt32</xsl:when>
          <xsl:when test="@type = 'u32v'">UInt32Array</xsl:when>
          <xsl:when test="@type = 'u64'">UInt64</xsl:when>
          <xsl:when test="@type = 'utf8v'">string</xsl:when>
          <xsl:when test="@type = 'u96'">LLRPBitArray</xsl:when>
          <xsl:when test="@type = 'bytesToEnd'">ByteArray</xsl:when>
          <xsl:otherwise>Unkown_Type</xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="DefineDefaultValue" match="field">
    <xsl:choose>
      <xsl:when test="@enumeration">;</xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="@type = 'u1'">=false;</xsl:when>
          <xsl:when test="@type = 'u1v'">=new LLRPBitArray();</xsl:when>
          <xsl:when test="@type = 'u2'">=new TwoBits(0);</xsl:when>
          <xsl:when test="@type = 'u8'">=0;</xsl:when>
          <xsl:when test="@type = 's8'">=0;</xsl:when>
          <xsl:when test="@type = 'u8v'">=new ByteArray();</xsl:when>
          <xsl:when test="@type = 'u16'">=0;</xsl:when>
          <xsl:when test="@type = 'u16v'">=new UInt16Array();</xsl:when>
          <xsl:when test="@type = 's16'">=0;</xsl:when>
          <xsl:when test="@type = 'u32'">=0;</xsl:when>
          <xsl:when test="@type = 'u32v'">=new UInt32Array();</xsl:when>
          <xsl:when test="@type = 'u64'">=0;</xsl:when>
          <xsl:when test="@type = 'utf8v'">=string.Empty;</xsl:when>
          <xsl:when test="@type = 'u96'">=new LLRPBitArray();</xsl:when>
          <xsl:when test="@type = 'bytesToEnd'">=new ByteArray();</xsl:when>
          <xsl:otherwise>Unkown_Type</xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="DefineDataLength" match="field">
    <xsl:choose>
      <xsl:when test="@enumeration and @type!='u8v'">
        private Int16 <xsl:value-of select="@name"/>_len = <xsl:choose>
          <xsl:when test="@type = 'u1'">1;</xsl:when>
          <xsl:when test="@type = 'u2'">2;</xsl:when>
          <xsl:when test="@type = 'u8'">8;</xsl:when>
          <xsl:when test="@type = 'u16'">16;</xsl:when>
          <xsl:when test="@type = 'u32'">32;</xsl:when>
          <xsl:otherwise>Unknown_Length;</xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@format='Hex'">
        private Int16 <xsl:value-of select="@name"/>_len;
      </xsl:when>
      <xsl:when test="@format='UTF8'">
        private Int16 <xsl:value-of select="@name"/>_len;
      </xsl:when>
      <xsl:otherwise>
        private Int16 <xsl:value-of select="@name"/>_len=0;
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="DefineParameterName" match="field">
    <xsl:choose>
      <xsl:when test="@name">
        <xsl:value-of select="@name"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@type"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="DefineParameterType" match="parameter">
    <xsl:choose>
      <xsl:when test="@repeat = '1'"></xsl:when>
      <xsl:when test="@repeat = '0-1'"></xsl:when>
      <xsl:when test="@repeat = '1-N'">[]</xsl:when>
      <xsl:when test="@repeat = '0-N'">[]</xsl:when>
      <xsl:otherwise>Unknown_Type</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
</xsl:stylesheet>