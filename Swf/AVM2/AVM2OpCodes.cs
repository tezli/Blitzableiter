using System;
using System.Collections.Generic;
using System.Text;

namespace Recurity.Swf.AVM2
{
    /// <summary>
    /// 
    /// </summary>
    public enum AVM2OpCodes : byte
    {
        // OP_bkpt = 0x01, // unused, ignored

        /// <summary>
        /// 
        /// </summary>
        OP_nop = 0x02,

        /// <summary>
        /// 
        /// </summary>
        OP_throw = 0x03,

        /// <summary>
        /// 
        /// </summary>
        OP_getsuper = 0x04,

        /// <summary>
        /// 
        /// </summary>
        OP_setsuper = 0x05,

        /// <summary>
        /// 
        /// </summary>
        OP_dxns = 0x06,

        /// <summary>
        /// 
        /// </summary>
        OP_dxnslate = 0x07,

        /// <summary>
        /// 
        /// </summary>
        OP_kill = 0x08,

        /// <summary>
        /// 
        /// </summary>
        OP_label = 0x09,

        /// <summary>
        /// 
        /// </summary>
        OP_ifnlt = 0x0C,

        /// <summary>
        /// 
        /// </summary>
        OP_ifnle = 0x0D,

        /// <summary>
        /// 
        /// </summary>
        OP_ifngt = 0x0E,

        /// <summary>
        /// 
        /// </summary>
        OP_ifnge = 0x0F,

        /// <summary>
        /// 
        /// </summary>
        OP_jump = 0x10,

        /// <summary>
        /// 
        /// </summary>
        OP_iftrue = 0x11,

        /// <summary>
        /// 
        /// </summary>
        OP_iffalse = 0x12,

        /// <summary>
        /// 
        /// </summary>
        OP_ifeq = 0x13,

        /// <summary>
        /// 
        /// </summary>
        OP_ifne = 0x14,

        /// <summary>
        /// 
        /// </summary>
        OP_iflt = 0x15,

        /// <summary>
        /// 
        /// </summary>
        OP_ifle = 0x16,

        /// <summary>
        /// 
        /// </summary>
        OP_ifgt = 0x17,

        /// <summary>
        /// 
        /// </summary>
        OP_ifge = 0x18,

        /// <summary>
        /// 
        /// </summary>
        OP_ifstricteq = 0x19,

        /// <summary>
        /// 
        /// </summary>
        OP_ifstrictne = 0x1A,

        /// <summary>
        /// 
        /// </summary>
        OP_lookupswitch = 0x1B,

        /// <summary>
        /// 
        /// </summary>
        OP_pushwith = 0x1C,

        /// <summary>
        /// 
        /// </summary>
        OP_popscope = 0x1D,

        /// <summary>
        /// 
        /// </summary>
        OP_nextname = 0x1E,

        /// <summary>
        /// 
        /// </summary>
        OP_hasnext = 0x1F,

        /// <summary>
        /// 
        /// </summary>
        OP_pushnull = 0x20,

        /// <summary>
        /// 
        /// </summary>
        OP_pushundefined = 0x21,

        /// <summary>
        /// 
        /// </summary>
        OP_nextvalue = 0x23,

        /// <summary>
        /// 
        /// </summary>
        OP_pushbyte = 0x24,

        /// <summary>
        /// 
        /// </summary>
        OP_pushshort = 0x25,

        /// <summary>
        /// 
        /// </summary>
        OP_pushtrue = 0x26,

        /// <summary>
        /// 
        /// </summary>
        OP_pushfalse = 0x27,

        /// <summary>
        /// 
        /// </summary>
        OP_pushnan = 0x28,

        /// <summary>
        /// 
        /// </summary>
        OP_pop = 0x29,

        /// <summary>
        /// 
        /// </summary>
        OP_dup = 0x2A,

        /// <summary>
        /// 
        /// </summary>
        OP_swap = 0x2B,

        /// <summary>
        /// 
        /// </summary>
        OP_pushstring = 0x2C,

        /// <summary>
        /// 
        /// </summary>
        OP_pushint = 0x2D,

        /// <summary>
        /// 
        /// </summary>
        OP_pushuint = 0x2E,

        /// <summary>
        /// 
        /// </summary>
        OP_pushdouble = 0x2F,

        /// <summary>
        /// 
        /// </summary>
        OP_pushscope = 0x30,

        /// <summary>
        /// 
        /// </summary>
        OP_pushnamespace = 0x31,

        /// <summary>
        /// 
        /// </summary>
        OP_hasnext2 = 0x32,

        // OP_lix8 = 0x33, // NEW internal only
        // OP_lix16 = 0x34, // NEW internal only


        /// <summary>
        /// 
        /// </summary>
        OP_li8 = 0x35,

        /// <summary>
        /// 
        /// </summary>
        OP_li16 = 0x36,

        /// <summary>
        /// 
        /// </summary>
        OP_li32 = 0x37,

        /// <summary>
        /// 
        /// </summary>
        OP_lf32 = 0x38,

        /// <summary>
        /// 
        /// </summary>
        OP_lf64 = 0x39,

        /// <summary>
        /// 
        /// </summary>
        OP_si8 = 0x3A,

        /// <summary>
        /// 
        /// </summary>
        OP_si16 = 0x3B,

        /// <summary>
        /// 
        /// </summary>
        OP_si32 = 0x3C,

        /// <summary>
        /// 
        /// </summary>
        OP_sf32 = 0x3D,

        /// <summary>
        /// 
        /// </summary>
        OP_sf64 = 0x3E,

        /// <summary>
        /// 
        /// </summary>
        OP_newfunction = 0x40,

        /// <summary>
        /// 
        /// </summary>
        OP_call = 0x41,

        /// <summary>
        /// 
        /// </summary>
        OP_construct = 0x42,

        /// <summary>
        /// 
        /// </summary>
        OP_callmethod = 0x43,

        /// <summary>
        /// 
        /// </summary>
        OP_callstatic = 0x44,

        /// <summary>
        /// 
        /// </summary>
        OP_callsuper = 0x45,

        /// <summary>
        /// 
        /// </summary>
        OP_callproperty = 0x46,

        /// <summary>
        /// 
        /// </summary>
        OP_returnvoid = 0x47,

        /// <summary>
        /// 
        /// </summary>
        OP_returnvalue = 0x48,

        /// <summary>
        /// 
        /// </summary>
        OP_constructsuper = 0x49,

        /// <summary>
        /// 
        /// </summary>
        OP_constructprop = 0x4A,

        /// <summary>
        /// 
        /// </summary>
        OP_callsuperid = 0x4B,

        /// <summary>
        /// 
        /// </summary>
        OP_callproplex = 0x4C,

        /// <summary>
        /// 
        /// </summary>
        OP_callinterface = 0x4D,

        /// <summary>
        /// 
        /// </summary>
        OP_callsupervoid = 0x4E,

        /// <summary>
        /// 
        /// </summary>
        OP_callpropvoid = 0x4F,

        /// <summary>
        /// 
        /// </summary>
        OP_sxi1 = 0x50,

        /// <summary>
        /// 
        /// </summary>
        OP_sxi8 = 0x51,

        /// <summary>
        /// 
        /// </summary>
        OP_sxi16 = 0x52,

        /// <summary>
        /// 
        /// </summary>
        OP_applytype = 0x53,

        /// <summary>
        /// 
        /// </summary>
        OP_newobject = 0x55,

        /// <summary>
        /// 
        /// </summary>
        OP_newarray = 0x56,

        /// <summary>
        /// 
        /// </summary>
        OP_newactivation = 0x57,

        /// <summary>
        /// 
        /// </summary>
        OP_newclass = 0x58,

        /// <summary>
        /// 
        /// </summary>
        OP_getdescendants = 0x59,

        /// <summary>
        /// 
        /// </summary>
        OP_newcatch = 0x5A,
        // OP_findpropglobalstrict = 0x5B, // NEW internal only
        // OP_findpropglobal = 0x5C, // NEW internal only

        /// <summary>
        /// 
        /// </summary>
        OP_findpropstrict = 0x5D,

        /// <summary>
        /// 
        /// </summary>
        OP_findproperty = 0x5E,

        /// <summary>
        /// 
        /// </summary>
        OP_finddef = 0x5F,

        /// <summary>
        /// 
        /// </summary>
        OP_getlex = 0x60,

        /// <summary>
        /// 
        /// </summary>
        OP_setproperty = 0x61,

        /// <summary>
        /// 
        /// </summary>
        OP_getlocal = 0x62,

        /// <summary>
        /// 
        /// </summary>
        OP_setlocal = 0x63,

        /// <summary>
        /// 
        /// </summary>
        OP_getglobalscope = 0x64,

        /// <summary>
        /// 
        /// </summary>
        OP_getscopeobject = 0x65,

        /// <summary>
        /// 
        /// </summary>
        OP_getproperty = 0x66,

        /// <summary>
        /// 
        /// </summary>
        OP_getouterscope = 0x67,

        /// <summary>
        /// 
        /// </summary>
        OP_initproperty = 0x68,

        /// <summary>
        /// 
        /// </summary>
        OP_deleteproperty = 0x6A,

        /// <summary>
        /// 
        /// </summary>
        OP_getslot = 0x6C,

        /// <summary>
        /// 
        /// </summary>
        OP_setslot = 0x6D,

        /// <summary>
        /// 
        /// </summary>
        OP_getglobalslot = 0x6E,

        /// <summary>
        /// 
        /// </summary>
        OP_setglobalslot = 0x6F,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_s = 0x70,

        /// <summary>
        /// 
        /// </summary>
        OP_esc_xelem = 0x71,

        /// <summary>
        /// 
        /// </summary>
        OP_esc_xattr = 0x72,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_i = 0x73,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_u = 0x74,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_d = 0x75,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_b = 0x76,

        /// <summary>
        /// 
        /// </summary>
        OP_convert_o = 0x77,

        /// <summary>
        /// 
        /// </summary>
        OP_checkfilter = 0x78,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce = 0x80,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_b = 0x81,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_a = 0x82,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_i = 0x83,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_d = 0x84,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_s = 0x85,

        /// <summary>
        /// 
        /// </summary>
        OP_astype = 0x86,

        /// <summary>
        /// 
        /// </summary>
        OP_astypelate = 0x87,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_u = 0x88,

        /// <summary>
        /// 
        /// </summary>
        OP_coerce_o = 0x89,

        /// <summary>
        /// 
        /// </summary>
        OP_negate = 0x90,

        /// <summary>
        /// 
        /// </summary>
        OP_increment = 0x91,

        /// <summary>
        /// 
        /// </summary>
        OP_inclocal = 0x92,

        /// <summary>
        /// 
        /// </summary>
        OP_decrement = 0x93,

        /// <summary>
        /// 
        /// </summary>
        OP_declocal = 0x94,

        /// <summary>
        /// 
        /// </summary>
        OP_typeof = 0x95,

        /// <summary>
        /// 
        /// </summary>
        OP_not = 0x96,

        /// <summary>
        /// 
        /// </summary>
        OP_bitnot = 0x97,

        /// <summary>
        /// 
        /// </summary>
        OP_add = 0xA0,

        /// <summary>
        /// 
        /// </summary>
        OP_subtract = 0xA1,

        /// <summary>
        /// 
        /// </summary>
        OP_multiply = 0xA2,

        /// <summary>
        /// 
        /// </summary>
        OP_divide = 0xA3,

        /// <summary>
        /// 
        /// </summary>
        OP_modulo = 0xA4,

        /// <summary>
        /// 
        /// </summary>
        OP_lshift = 0xA5,

        /// <summary>
        /// 
        /// </summary>
        OP_rshift = 0xA6,

        /// <summary>
        /// 
        /// </summary>
        OP_urshift = 0xA7,

        /// <summary>
        /// 
        /// </summary>
        OP_bitand = 0xA8,

        /// <summary>
        /// 
        /// </summary>
        OP_bitor = 0xA9,

        /// <summary>
        /// 
        /// </summary>
        OP_bitxor = 0xAA,

        /// <summary>
        /// 
        /// </summary>
        OP_equals = 0xAB,

        /// <summary>
        /// 
        /// </summary>
        OP_strictequals = 0xAC,

        /// <summary>
        /// 
        /// </summary>
        OP_lessthan = 0xAD,

        /// <summary>
        /// 
        /// </summary>
        OP_lessequals = 0xAE,

        /// <summary>
        /// 
        /// </summary>
        OP_greaterthan = 0xAF,

        /// <summary>
        /// 
        /// </summary>
        OP_greaterequals = 0xB0,

        /// <summary>
        /// 
        /// </summary>
        OP_instanceof = 0xB1,

        /// <summary>
        /// 
        /// </summary>
        OP_istype = 0xB2,

        /// <summary>
        /// 
        /// </summary>
        OP_istypelate = 0xB3,

        /// <summary>
        /// 
        /// </summary>
        OP_in = 0xB4,

        /// <summary>
        /// 
        /// </summary>
        OP_increment_i = 0xC0,

        /// <summary>
        /// 
        /// </summary>
        OP_decrement_i = 0xC1,

        /// <summary>
        /// 
        /// </summary>
        OP_inclocal_i = 0xC2,

        /// <summary>
        /// 
        /// </summary>
        OP_declocal_i = 0xC3,

        /// <summary>
        /// 
        /// </summary>
        OP_negate_i = 0xC4,

        /// <summary>
        /// 
        /// </summary>
        OP_add_i = 0xC5,

        /// <summary>
        /// 
        /// </summary>
        OP_subtract_i = 0xC6,

        /// <summary>
        /// 
        /// </summary>
        OP_multiply_i = 0xC7,

        /// <summary>
        /// 
        /// </summary>
        OP_getlocal0 = 0xD0,

        /// <summary>
        /// 
        /// </summary>
        OP_getlocal1 = 0xD1,

        /// <summary>
        /// 
        /// </summary>
        OP_getlocal2 = 0xD2,

        /// <summary>
        /// 
        /// </summary>
        OP_getlocal3 = 0xD3,

        /// <summary>
        /// 
        /// </summary>
        OP_setlocal0 = 0xD4,

        /// <summary>
        /// 
        /// </summary>
        OP_setlocal1 = 0xD5,

        /// <summary>
        /// 
        /// </summary>
        OP_setlocal2 = 0xD6,

        /// <summary>
        /// 
        /// </summary>
        OP_setlocal3 = 0xD7,

        /// <summary>
        /// 
        /// </summary>
        OP_abs_jump = 0xEE,

        /// <summary>
        /// 
        /// </summary>
        OP_debug = 0xEF,

        /// <summary>
        /// 
        /// </summary>
        OP_debugline = 0xF0,

        /// <summary>
        /// 
        /// </summary>
        OP_debugfile = 0xF1 // ,
        // OP_bkptline = 0xF2, // unused, ignored
        // OP_timestamp = 0xF3, // unused, ignored
        // OP_restargc = 0xF4,  // internal only
        // OP_restarg = 0xF5  // internal only
    }
}
