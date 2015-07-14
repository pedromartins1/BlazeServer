using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;

namespace BlazeServer
{
    public class Utilities
    {
        public static MemoryStream StringToStream(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(content);
            writer.Flush();

            return stream;
        }

        public static uint SwapBytes(uint word)
        {
            // convert big endian to little endian
            return ((word >> 24) & 0x000000FF) | ((word >> 8) & 0x0000FF00) | ((word << 8) & 0x00FF0000) | ((word << 24) & 0xFF000000);
        }

        public static ulong GetUnixTime()
        {
            return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        public static long IPToLong(string ip)
        {
            return (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(ip).Address);
        }

        public static Dictionary<string, Tdf> DecodePayload(byte[] payload)
        {
            var decoder = new TdfDecoder(payload);

            return decoder.Decode();
        }

        public static readonly string[] stats = new string[] { "r01_00", "r01_01", "r05_00", "r05_01", "r04_00", "r04_01", "r03_00", "r03_01", "r02_00", "r02_01", "r10_00", "r10_01", "r08_00", "r08_01", "r07_00", "r07_01", "r06_00", "r06_01", "r09_00", "r09_01", "r11_00", "r11_01", "r12_00", "r12_01", "r13_00", "r13_01", "r14_00", "r14_01", "r15_00", "r15_01", "r16_00", "r16_01", "r17_00", "r17_01", "r18_00", "r18_01", "r19_00", "r19_01", "r20_00", "r20_01", "r21_00", "r21_01", "r22_00", "r22_01", "r23_00", "r23_01", "r24_00", "r24_01", "r25_00", "r25_01", "r26_00", "r26_01", "r27_00", "r27_01", "r28_00", "r28_01", "r29_00", "r29_01", "r30_00", "r30_01", "r31_00", "r31_01", "r32_00", "r32_01", "r33_00", "r33_01", "r35_00", "r35_01", "r36_00", "r36_01", "r37_00", "r37_01", "r34_00", "r34_01", "r38_00", "r38_01", "r39_00", "r39_01", "r40_00", "r40_01", "r41_00", "r41_01", "r42_00", "r42_01", "r43_00", "r43_01", "r44_00", "r44_01", "r45_00", "r45_01", "xp3rts_00", "xp3rts_01", "xp2rgm_00", "xp2rgm_01", "xp3rdom_00", "xp3rdom_01", "xp4rscav_00", "xp4rscav_01", "xp2rtdmc_00", "xp2rtdmc_01", "xp4rndom_00", "xp4rndom_01", "xp4rnscv_00", "xp4rnscv_01", "xp3rnts_00", "xp3rnts_01", "xp2rntdmcq_00", "xp2rntdmcq_01", "xp3rngm_00", "xp3rngm_01", "m01_00", "m01_01", "m02_00", "m02_01", "m03_00", "m03_01", "m04_00", "m04_01", "m05_00", "m05_01", "m06_00", "m06_01", "m07_00", "m07_01", "m08_00", "m08_01", "m09_00", "m09_01", "m10_00", "m10_01", "m11_00", "m11_01", "m12_00", "m12_01", "m13_00", "m13_01", "m14_00", "m14_01", "m15_00", "m15_01", "m16_00", "m16_01", "m17_00", "m17_01", "m18_00", "m18_01", "m19_00", "m19_01", "m20_00", "m20_01", "m21_00", "m21_01", "m22_00", "m22_01", "m23_00", "m23_01", "m24_00", "m24_01", "m25_00", "m25_01", "m26_00", "m26_01", "m27_00", "m27_01", "m28_00", "m28_01", "m29_00", "m29_01", "m30_00", "m30_01", "m31_00", "m31_01", "m32_00", "m32_01", "m33_00", "m33_01", "m34_00", "m34_01", "m35_00", "m35_01", "m36_00", "m36_01", "m37_00", "m37_01", "c_m37_seqM224__ki_g", "m38_00", "m38_01", "c_m38___tad_g", "m39_00", "m39_01", "c_m39_waeClay__kwa_g", "m40_00", "m40_01", "c_m40_seqRad__sv_g", "m41_00", "m41_01", "c_m41_enu_rcu_suu_asu___sa_g", "m42_00", "m42_01", "c_m42_enr_rcr_sur_asr___sa_g", "m43_00", "m43_01", "c_m43_as__sa_g", "m44_00", "m44_01", "c_m44_en__sa_g", "m45_00", "m45_01", "c_m45_su__sa_g", "m46_00", "m46_01", "c_m46_rc__sa_g", "m47_00", "m47_01", "c_m47_vMBT__si_g", "m48_00", "m48_01", "c_m48_vmaH__si_g", "m49_00", "m49_01", "c_m49_vmaJ__si_g", "m50_00", "m50_01", "c_m50_sw__si_g", "xp3mts_00", "xp3mts_01", "xp2mdom_00", "xp2mdom_01", "xp2mgm_00", "xp2mgm_01", "xp4mscv_00", "xp4mscv_01", "xp2mtdmcq_00", "xp2mtdmcq_01", "ssclbas_00", "ssclbas_01", "c_ssclbas_bas__ghb_ghva", "ssclbe_00", "ssclbe_01", "c_ssclbe_be__ghb_ghva", "ssclbr_00", "ssclbr_01", "c_ssclbr_br__ghb_ghva", "ssclbsu_00", "ssclbsu_01", "c_ssclbsu_bsu__ghb_ghva", "ssclbvmbt_00", "ssclbvmbt_01", "c_ssclbvmbt_bvmbt__ghb_ghva", "ssclbvifv_00", "ssclbvifv_01", "c_ssclbvifv_bvifv__ghb_ghva", "ssclbvaa_00", "ssclbvaa_01", "c_ssclbvaa_bvaa__ghb_ghva", "ssclbvah_00", "ssclbvah_01", "c_ssclbvah_bvah__ghb_ghva", "ssclbvsh_00", "ssclbvsh_01", "c_ssclbvsh_bvsh__ghb_ghva", "ssclbvjet_00", "ssclbvjet_01", "c_ssclbvjet_bvjet__ghb_ghva", "ssclbvart_00", "ssclbvart_01", "c_ssclbvart_bvart__ghb_ghva", "ssclbvlbt_00", "ssclbvlbt_01", "c_ssclbvlbt_bvlbt__ghb_ghva", "sshaarAEK_00", "sshaarAEK_01", "c_sshaarAEK_arAEK__kwa_g", "sshaarAK74_00", "sshaarAK74_01", "c_sshaarAK74_arAK74__kwa_g", "sshaarAN94_00", "sshaarAN94_01", "c_sshaarAN94_arAN94__kwa_g", "sshaarKH_00", "sshaarKH_01", "c_sshaarKH_arKH__kwa_g", "sshaarF2_00", "sshaarF2_01", "c_sshaarF2_arF2__kwa_g", "sshaarG3_00", "sshaarG3_01", "c_sshaarG3_arG3__kwa_g", "sshaarM16_00", "sshaarM16_01", "c_sshaarM16_arM16__kwa_g", "sshaarM416_00", "sshaarM416_01", "c_sshaarM416_arM416__kwa_g", "sshacaA91_00", "sshacaA91_01", "c_sshacaA91_caA91__kwa_g", "sshacaAKS_00", "sshacaAKS_01", "c_sshacaAKS_caAKS__kwa_g", "sshacaG36_00", "sshacaG36_01", "c_sshacaG36_caG36__kwa_g", "sshacaM4_00", "sshacaM4_01", "c_sshacaM4_caM4__kwa_g", "sshacaSCAR_00", "sshacaSCAR_01", "c_sshacaSCAR_caSCAR__kwa_g", "sshacaSG553_00", "sshacaSG553_01", "c_sshacaSG553_caSG553__kwa_g", "sshamgPech_00", "sshamgPech_01", "c_sshamgPech_mgPech__kwa_g", "sshamgRPK_00", "sshamgRPK_01", "c_sshamgRPK_mgRPK__kwa_g", "sshamgT88_00", "sshamgT88_01", "c_sshamgT88_mgT88__kwa_g", "sshamgM240_00", "sshamgM240_01", "c_sshamgM240_mgM240__kwa_g", "sshamgM249_00", "sshamgM249_01", "c_sshamgM249_mgM249__kwa_g", "sshamgM27_00", "sshamgM27_01", "c_sshamgM27_mgM27__kwa_g", "sshamgM60_00", "sshamgM60_01", "c_sshamgM60_mgM60__kwa_g", "sshasgSaiga_00", "sshasgSaiga_01", "c_sshasgSaiga_sgSaiga__kwa_g", "sshasgUSAS_00", "sshasgUSAS_01", "c_sshasgUSAS_sgUSAS__kwa_g", "sshasg870_00", "sshasg870_01", "c_sshasg870_sg870__kwa_g", "sshasgDAO_00", "sshasgDAO_01", "c_sshasgDAO_sgDAO__kwa_g", "sshasgM1014_00", "sshasgM1014_01", "c_sshasgM1014_sgM1014__kwa_g", "sshasrSV98_00", "sshasrSV98_01", "c_sshasrSV98_srSV98__kwa_g", "sshasrSKS_00", "sshasrSKS_01", "c_sshasrSKS_srSKS__kwa_g", "sshasrSVD_00", "sshasrSVD_01", "c_sshasrSVD_srSVD__kwa_g", "sshasrM40_00", "sshasrM40_01", "c_sshasrM40_srM40__kwa_g", "sshasrM98_00", "sshasrM98_01", "c_sshasrM98_srM98__kwa_g", "sshasrM39_00", "sshasrM39_01", "c_sshasrM39_srM39__kwa_g", "sshasrMK11_00", "sshasrMK11_01", "c_sshasrMK11_srMK11__kwa_g", "sshasmPP2000_00", "sshasmPP2000_01", "c_sshasmPP2000_smPP2000__kwa_g", "sshasmMP7_00", "sshasmMP7_01", "c_sshasmMP7_smMP7__kwa_g", "sshasmP90_00", "sshasmP90_01", "c_sshasmP90_smP90__kwa_g", "sshasmPDR_00", "sshasmPDR_01", "c_sshasmPDR_smPDR__kwa_g", "sshasmUMP_00", "sshasmUMP_01", "c_sshasmUMP_smUMP__kwa_g", "sshapM9_00", "sshapM9_01", "c_sshapM9_pM9__kwa_g", "sshapG17_00", "sshapG17_01", "c_sshapG17_pG17__kwa_g", "sshapg18_00", "sshapg18_01", "c_sshapg18_pg18__kwa_g", "sshapM93R_00", "sshapM93R_01", "c_sshapM93R_pM93R__kwa_g", "sshapTaur_00", "sshapTaur_01", "c_sshapTaur_pTaur__kwa_g", "sshapM412_00", "sshapM412_01", "c_sshapM412_pM412__kwa_g", "sshapMP443_00", "sshapMP443_01"
                , "c_sshapMP443_pMP443__kwa_g", "sshapM1911_00", "sshapM1911_01", "c_sshapM1911_pM1911__kwa_g", "sshawahUSG_00", "sshawahUSG_01", "c_sshawahUSG_wahUSG__kwa_g", "sshawahUGL_00", "sshawahUGL_01", "c_sshawahUGL_wahUGL__kwa_g", "sshawLATSMAW_00", "sshawLATSMAW_01", "c_sshawLATSMAW_wLATSMAW__kwa_g", "sshawLATRPG_00", "sshawLATRPG_01", "c_sshawLATRPG_wLATRPG__kwa_g", "sshawLATJAV_00", "sshawLATJAV_01", "c_sshawLATJAV_wLATJAV__kwa_g", "sshawLAAFIM_00", "sshawLAAFIM_01", "c_sshawLAAFIM_wLAAFIM__kwa_g", "sshawLAAIGL_00", "sshawLAAIGL_01", "c_sshawLAAIGL_wLAAIGL__kwa_g", "sshawasK_00", "sshawasK_01", "c_sshawasK_wasK__kwa_g", "sshapMP443S_00", "sshapMP443S_01", "c_sshapMP443S_pMP443S__kwa_g", "sshapM9S_00", "sshapM9S_01", "c_sshapM9S_pM9S__kwa_g", "sshapTaurS_00", "sshapTaurS_01", "c_sshapTaurS_pTaurS__kwa_g", "sshapM1911S_00", "sshapM1911S_01", "c_sshapM1911S_pM1911S__kwa_g", "sshapM1911L_00", "sshapM1911L_01", "c_sshapM1911L_pM1911L__kwa_g", "sshapM1911T_00", "sshapM1911T_01", "c_sshapM1911T_pM1911T__kwa_g", "sshapMP443L_00", "sshapMP443L_01", "c_sshapMP443L_pMP443L__kwa_g", "sshapG17S_00", "sshapG17S_01", "c_sshapG17S_pG17S__kwa_g", "sshapg18S_00", "sshapg18S_01", "c_sshapg18S_pg18S__kwa_g", "sshasmVAL_00", "sshasmVAL_01", "c_sshasmVAL_smVAL__kwa_g", "sshawaeC4_00", "sshawaeC4_01", "c_sshawaeC4_waeC4__kwa_g", "sshawaeMine_00", "sshawaeMine_01", "c_sshawaeMine_waeMine__kwa_g", "sshawaeClay_00", "sshawaeClay_01", "c_sshawaeClay_waeClay__kwa_g", "sshaarFAMAS_00", "sshaarFAMAS_01", "c_sshaarFAMAS_arFAMAS__kwa_g", "sshacaHK53_00", "sshacaHK53_01", "c_sshacaHK53_caHK53__kwa_g", "sshasgJackH_00", "sshasgJackH_01", "c_sshasgJackH_sgJackH__kwa_g", "sshaarL85A2_00", "sshaarL85A2_01", "c_sshaarL85A2_arL85A2__kwa_g", "sshasrL96_00", "sshasrL96_01", "c_sshasrL96_srL96__kwa_g", "sshamgMG36_00", "sshamgMG36_01", "c_sshamgMG36_mgMG36__kwa_g", "sshasmPP19_00", "sshasmPP19_01", "c_sshasmPP19_smPP19__kwa_g", "sshamgQBB95_00", "sshamgQBB95_01", "c_sshamgQBB95_mgQBB95__kwa_g", "sshacaQBZ95B_00", "sshacaQBZ95B_01", "c_sshacaQBZ95B_caQBZ95B__kwa_g", "sshasrQBU88_00", "sshasrQBU88_01", "c_sshasrQBU88_srQBU88__kwa_g", "sshacaACR_00", "sshacaACR_01", "c_sshacaACR_caACR__kwa_g", "sshaarAUG_00", "sshaarAUG_01", "c_sshaarAUG_arAUG__kwa_g", "sshaarSCARL_00", "sshaarSCARL_01", "c_sshaarSCARL_arSCARL__kwa_g", "sshacaMTAR21_00", "sshacaMTAR21_01", "c_sshacaMTAR21_caMTAR21__kwa_g", "sshamgL86A1_00", "sshamgL86A1_01", "c_sshamgL86A1_mgL86A1__kwa_g", "sshamgLSAT_00", "sshamgLSAT_01", "c_sshamgLSAT_mgLSAT__kwa_g", "sshasrHK417_00", "sshasrHK417_01", "c_sshasrHK417_srHK417__kwa_g", "sshasrJNG90_00", "sshasrJNG90_01", "c_sshasrJNG90_srJNG90__kwa_g", "sshasmM5K_00", "sshasmM5K_01", "c_sshasmM5K_smM5K__kwa_g", "sshasgSPAS12_00", "sshasgSPAS12_01", "c_sshasgSPAS12_sgSPAS12__kwa_g", "sshawasXBK_00", "sshawasXBK_01", "c_sshawasXBK_wasXBK__kwa_g", "sshawasXBS_00", "sshawasXBS_01", "c_sshawasXBS_wasXBS__kwa_g", "sscobc_00", "sscobc_01", "c_sscobc_bc__ghb_ghva", "ACH32_00", "ACH32_01", "ACH34_00", "ACH34_01", "ACH35_00", "ACH35_01", "ACH37_00", "ACH37_01", "ACH33_00", "ACH33_01", "ACH40_00", "ACH40_01", "c_ACH40___ru_ghva", "ACH38_00", "ACH38_01", "ACH39_00", "ACH39_01", "ACH36_00", "ACH36_01", "dtb001_00", "dtb001_01", "dtb002_00", "dtb002_01", "dtb003_00", "dtb003_01", "dtb004_00", "dtb004_01", "dtb005_00", "dtb005_01", "dtb006_00", "dtb006_01", "dtb007_00", "dtb007_01", "dtb008_00", "dtb008_01", "dtb009_00", "dtb009_01", "dtb010_00", "dtb010_01", "dtb011_00", "dtb011_01", "dtb012_00", "dtb012_01", "dtb013_00", "dtb013_01", "dtb014_00", "dtb014_01", "dtb015_00", "dtb015_01", "dtb016_00", "dtb016_01", "dtb017_00", "dtb017_01", "dtb018_00", "dtb018_01", "dtb019_00", "dtb019_01", "dtb020_00", "dtb020_01", "dtb021_00", "dtb021_01", "dtb022_00", "dtb022_01", "dtb023_00", "dtb023_01", "dtb024_00", "dtb024_01", "dtb025_00", "dtb025_01", "dtb026_00", "dtb026_01", "dtb027_00", "dtb027_01", "dtb028_00", "dtb028_01", "dtb029_00", "dtb029_01", "dtb030_00", "dtb030_01", "dtb031_00", "dtb031_01", "dtb032_00", "dtb032_01", "dtb033_00", "dtb033_01", "dtb034_00", "dtb034_01", "dtb035_00", "dtb035_01", "dtb036_00", "dtb036_01", "dtb037_00", "dtb037_01", "dtb038_00", "dtb038_01", "dtb039_00", "dtb039_01", "dtb040_00", "dtb040_01", "MBTm_00", "MBTm_01", "c_MBTm_bvmbt__ghb_ghva", "AAm_00", "AAm_01", "c_AAm_bvaa__ghb_ghva", "JETm_00", "JETm_01", "c_JETm_bvjet__ghb_ghva", "SCTm_00", "SCTm_01", "c_SCTm_bvsh__ghb_ghva", "IFVm_00", "IFVm_01", "c_IFVm_bvifv__ghb_ghva", "AHm_00", "AHm_01", "MARTm_00", "MARTm_01", "c_MARTm_bvart__ghb_ghva", "TDm_00", "TDm_01", "c_TDm_bvlbt__ghb_ghva", "xp3dtbwints_00", "xp3dtbwints_01", "xp4dtbwinscv_00", "xp4dtbwinscv_01", "xp2dtbngm_00", "xp2dtbngm_01", "xp2dtbwindom_00", "xp2dtbwindom_01", "xp2dtbndom_00", "xp2dtbndom_01", "xp2dtbwingm_00", "xp2dtbwingm_01", "xp3dtbnts_00", "xp3dtbnts_01", "xp2dtbntdmcq_00", "xp2dtbntdmcq_01", "xp2dbtwintdmcq_00", "xp2dbtwintdmcq_01", "xp4dtbnscv_00", "xp4dtbnscv_01", "xpma03_00", "xpma03_01", "c_xpma03___r_g", "c_xpma03_wasRT__kwa_g", "xpma04_00", "xpma04_01", "c_xpma04_wahLAT__kwa_g", "c_xpma04_vA_wasRT_diw_g", "c_xpma04_mwin_mgc_roo_g", "xpma07_00", "xpma07_01", "c_xpma07_wahSR__kwa_g", "c_xpma07___tx_g", "xpma08_00", "xpma08_01", "c_xpma08___hsh_g", "c_xpma08___sp_g", "c_xpma08___dt_g", "xpma01_00", "xpma01_01", "c_xpma01___re_g", "c_xpma01___h_g", "xpma02_00", "xpma02_01", "c_xpma02_wahA__kwa_g", "c_xpma02_wahUGL__kwa_g", "c_xpma02_mwin_mgsd_roo_g", "xpma09_00", "xpma09_01", "c_xpma09___ca_g", "c_xpma09___ccp_g", "c_xpma09_xp11__so_g", "xpma10_00", "xpma10_01", "c_xpma10_smPP19__kwa_g", "c_xpma10_trDpv__ki_g", "c_xpma10_ifvBTR90__ki_g", "c_xpma10_xp13__so_g", "c_xpma10_xp12__so_g", "xpma06_00", "xpma06_01", "c_xpma06_wahM__kwa_g", "c_xpma06___sua_g", "c_xpma06___rs_g", "xpma05_00", "xpma05_01", "c_xpma05_wahM__kwa_g", "c_xpma05_seqM224__ki_g", "XPACH01_00", "XPACH01_01", "XPACH02_00", "XPACH02_01", "c_XPACH02_ifvBTR90__ki_g", "c_XPACH02_trDpv__ki_g", "c_XPACH02_jfx1F35JSF__ki_g", "XPACH03_00", "XPACH03_01", "c_XPACH03_arFAMAS__kwa_g", "c_XPACH03_caHK53__kwa_g"
                , "c_XPACH03_sgJackH__kwa_g", "c_XPACH03_arL85A2__kwa_g", "c_XPACH03_srL96__kwa_g", "c_XPACH03_mgMG36__kwa_g", "c_XPACH03_smPP19__kwa_g", "c_XPACH03_mgQBB95__kwa_g", "c_XPACH03_srQBU88__kwa_g", "c_XPACH03_caQBZ95B__kwa_g", "XPACH04_00", "XPACH04_01", "c_XPACH04_trSkid__ki_ghva", "XPACH05_00", "XPACH05_01", "c_XPACH05_XPACH05__m_ghva", "xp2ma02_00", "xp2ma02_01", "c_xp2ma02_wahU__kwa_g", "c_xp2ma02_waeM67__kwa_g", "xp2ma09_00", "xp2ma09_01", "c_xp2ma09_whP__kwa_g", "c_xp2ma09_wahSG__kwa_g", "xp2ma06_00", "xp2ma06_01", "c_xp2ma06_seqEOD__ki_g", "c_xp2ma06_wahC__kwa_g", "xp2ma03_00", "xp2ma03_01", "c_xp2ma03___sr_g", "c_xp2ma03_wahM__kwa_g", "xp2ma07_00", "xp2ma07_01", "c_xp2ma07_seqUGS__spx_g", "c_xp2ma07___ccp_g", "xp2ma04_00", "xp2ma04_01", "c_xp2ma04_waeC4__kwa_g", "c_xp2ma04___dt_g", "xp2ma05_00", "xp2ma05_01", "c_xp2ma05_wahLAT__kwa_g", "c_xp2ma05_wahC__kwa_g", "xp2ma08_00", "xp2ma08_01", "c_xp2ma08_wahSR__kwa_g", "c_xp2ma08_mwin_mgdom_roo_g", "xp2ma01_00", "xp2ma01_01", "c_xp2ma01___sre_g", "c_xp2ma01_wahA__kwa_g", "xp2ma10_00", "xp2ma10_01", "c_xp2ma10_t5_mggm_psy_g", "c_xp2ma10_wahSM__kwa_g", "XP2ACH01_00", "XP2ACH01_01", "c_XP2ACH01_mwin_mgdom_roo_g", "XP2ACH04_00", "XP2ACH04_01", "XP2ACH03_00", "XP2ACH03_01", "c_XP2ACH03_XP2ACH03__m_ghva", "XP2ACH05_00", "XP2ACH05_01", "c_XP2ACH05_caACR__kwa_g", "c_XP2ACH05_srHK417__kwa_g", "c_XP2ACH05_srJNG90__kwa_g", "c_XP2ACH05_mgL86A1__kwa_g", "c_XP2ACH05_mgLSAT__kwa_g", "c_XP2ACH05_smM5K__kwa_g", "c_XP2ACH05_caMTAR21__kwa_g", "c_XP2ACH05_arSCARL__kwa_g", "c_XP2ACH05_sgSPAS12__kwa_g", "c_XP2ACH05_arAUG__kwa_g", "XP2ACH02_00", "XP2ACH02_01", "c_XP2ACH02__mggm_roo_g", "xp5r502_00", "xp5r502_01", "xp5r501_00", "xp5r501_01", "xp5ras_00", "xp5ras_01", "xp5asw_00", "xp5asw_01", "xp5m501_00", "xp5m501_01", "xp5mas_00", "xp5mas_01", "DTAxp5moto_00", "DTAxp5moto_01", "c_DTAxp5moto_trKLR__jix_g", "c_DTAxp5moto_trKLR__kfx_g", "DTAxp5fr_00", "DTAxp5fr_01", "c_DTAxp5fr___fct_g", "XP5dtapanc_00", "XP5dtapanc_01", "c_XP5dtapanc___ffd_g", "xp5dtbsctf_00", "xp5dtbsctf_01", "xp5dtbwinsctf_00", "xp5dtbwinsctf_01", "xp5dtbas_00", "xp5dtbas_01", "xp5dtbwinsas_00", "xp5dtbwinsas_01", "xp5bf3pwm_00", "xp5bf3pwm_01", "c_xp5bf3pwm_wahA__kwa_g", "c_xp5bf3pwm_wahC__kwa_g", "c_xp5bf3pwm_wahM__kwa_g", "c_xp5bf3pwm_wahSG__kwa_g", "c_xp5bf3pwm_wahSR__kwa_g", "c_xp5bf3pwm_wahSM__kwa_g", "xp5bf3hg_00", "xp5bf3hg_01", "c_xp5bf3hg_whP__kwa_g", "xp5bf3ce_00", "xp5bf3ce_01", "xp5bf3sw_00", "xp5bf3sw_01", "xp5bf3bq_00", "xp5bf3bq_01", "xp2prema03_00", "xp2prema03_01", "c_xp2prema03_srL96__kwa_g", "c_xp2prema03__srL96_hsh_g", "c_xp2prema03_seqRad__sv_g", "xp2prema07_00", "xp2prema07_01", "c_xp2prema07_mgPech__kwa_g", "c_xp2prema07_vA_waeC4_diw_g", "c_xp2prema07_waeClay__kwa_g", "xp2prema08_00", "xp2prema08_01", "c_xp2prema08_srL96__kwa_g", "c_xp2prema08_seqMAV__spx_g", "c_xp2prema08_srL96__hsd_ghvp", "xp2prema01_00", "xp2prema01_01", "c_xp2prema01_arF2__kwa_g", "c_xp2prema01__arF2_hsh_g", "c_xp2prema01___qh_g", "xp2prema02_00", "xp2prema02_01", "c_xp2prema02_mgPech__kwa_g", "c_xp2prema02___sr_g", "xp2prema09_00", "xp2prema09_01", "c_xp2prema09_caSCAR__kwa_g", "c_xp2prema09_waeMine__kwa_g", "c_xp2prema09_vmA_wahLAT_diw_g", "xp2prema06_00", "xp2prema06_01", "c_xp2prema06_arF2__kwa_g", "c_xp2prema06___sre_g", "c_xp2prema06_wahUSG__kwa_g", "xp2prema04_00", "xp2prema04_01", "c_xp2prema04_caSCAR__kwa_g", "c_xp2prema04___sqr_g", "c_xp2prema04_wahSG__kwa_g", "xp2prema05_00", "xp2prema05_01", "c_xp2prema05_wahA__kwa_g", "c_xp2prema05_wahC__kwa_g", "c_xp2prema05_wahSR__kwa_g", "c_xp2prema05_wahM__kwa_g", "c_xp2prema05_whP__kwa_g", "xp2prema10_00", "xp2prema10_01", "c_xp2prema10_as__ks_g", "c_xp2prema10_en__ks_g", "c_xp2prema10_su__ks_g", "c_xp2prema10_rc__ks_g", "xp3ma01_00", "xp3ma01_01", "c_xp3ma01_vTD__de_g", "c_xp3ma01_vMART__de_g", "xp3ma05_00", "xp3ma05_01", "c_xp3ma05_vmaG__ki_g", "xp3ma04_00", "xp3ma04_01", "c_xp3ma04_vMART__ki_g", "xp3ma03_00", "xp3ma03_01", "c_xp3ma03___r_g", "xp3ma02_00", "xp3ma02_01", "c_xp3ma02_vTD__ki_g", "xp3prema07_00", "xp3prema07_01", "c_xp3prema07_caMTAR21__kwa_g", "c_xp3prema07_vA_seqEOD_di_g", "c_xp3prema07_cr10__ga_g", "xp3prema02_00", "xp3prema02_01", "c_xp3prema02_vIFV__ki_g", "xp3prema09_00", "xp3prema09_01", "c_xp3prema09_srSKS__kwa_g", "c_xp3prema09___tad_g", "c_xp3prema09_cr11__ga_g", "xp3prema01_00", "xp3prema01_01", "c_xp3prema01_vMBT__ki_g", "xp3prema06_00", "xp3prema06_01", "c_xp3prema06_arL85A2__kwa_g", "c_xp3prema06__arL85A2_hsh_g", "c_xp3prema06_cr01__ga_g", "xp3prema03_00", "xp3prema03_01", "c_xp3prema03_vmaH__ki_g", "c_xp3prema03_vMBT_vmaH_di_g", "xp3prema10_00", "xp3prema10_01", "c_xp3prema10_smUMP_as_kwa_g", "c_xp3prema10_smUMP_su_kwa_g", "c_xp3prema10_smUMP_en_kwa_g", "c_xp3prema10_smUMP_rc_kwa_g", "xp3prema05_00", "xp3prema05_01", "c_xp3prema05_vmT__ki_g", "c_xp3prema05___sda_g", "xp3prema08_00", "xp3prema08_01", "c_xp3prema08_mgLSAT__kwa_g", "c_xp3prema08_seqM224__ki_g", "c_xp3prema08_cr15__ga_g", "xp3prema04_00", "xp3prema04_01", "c_xp3prema04_vmaJ__ki_g", "c_xp3prema04_vmaH_vmaJ_di_g", "XP3ACH01_00", "XP3ACH01_01", "c_XP3ACH01_vmaG__ki_g", "XP3ACH05_00", "XP3ACH05_01", "c_XP3ACH05_vTD__ki_g", "c_XP3ACH05_vMART__ki_g", "XP3ACH03_00", "XP3ACH03_01", "c_XP3ACH03_trQB__ki_g", "XP3ACH04_00", "XP3ACH04_01", "c_XP3ACH04_mwin_mgts_roo_g", "XP3ACH02_00", "XP3ACH02_01", "c_XP3ACH02_vmaG__de_g", "xp4ma05_00", "xp4ma05_01", "c_xp4ma05___sp_g", "c_xp4ma05___tx_g", "c_xp4ma05___tad_g", "xp4ma01_00", "xp4ma01_01", "c_xp4ma01_mwin_mgscv_roo_g", "xp4ma02_00", "xp4ma02_01", "c_xp4ma02_wasXB__kwa_g", "xp4ma10_00", "xp4ma10_01", "c_xp4ma10_xp4l1__so_g", "c_xp4ma10_xp4l2__so_g", "c_xp4ma10_xp4l4__so_g", "c_xp4ma10_xp4l3__so_g", "c_xp4ma10_XP4ACH02__m_g", "xp4ma06_00", "xp4ma06_01", "c_xp4ma06_wahSR__kwa_g", "c_xp4ma06__whP_hsh_g", "xp4ma07_00", "xp4ma07_01", "c_xp4ma07_waeM67__kwa_g", "c_xp4ma07_wahUGL__kwa_g", "c_xp4ma07_waeC4__kwa_g", "xp4ma09_00", "xp4ma09_01", "c_xp4ma09_wasXB__hsd_g", "c_xp4ma09_wasXB__kwa_g", "xp4ma03_00", "xp4ma03_01", "c_xp4ma03_wahC__kwa_g", "c_xp4ma03_wahA__kwa_g", "xp4ma04_00", "xp4ma04_01"
                , "c_xp4ma04_wahSR__hsd_g", "c_xp4ma04_wahA__hsd_g", "c_xp4ma04_wahC__hsd_g", "xp4ma08_00", "xp4ma08_01", "c_xp4ma08_trHmvM__ki_g", "c_xp4ma08_trVodnM__ki_g", "c_xp4ma08_trVanM__ki_g", "XP4ACH01_00", "XP4ACH01_01", "c_XP4ACH01_cXP4XbowBA__ga_g", "c_XP4ACH01_cXP4XbowR__ga_g", "c_XP4ACH01_cXP4XbowHE__ga_g", "c_XP4ACH01_cXP4XbowS__ga_g", "XP4ACH05_00", "XP4ACH05_01", "c_XP4ACH05_XP4ACH05__m_g", "XP4ACH03_00", "XP4ACH03_01", "c_XP4ACH03_vA_wasXB_diw_g", "XP4ACH04_00", "XP4ACH04_01", "XP4ACH02_00", "XP4ACH02_01", "c_XP4ACH02_XP4ACH02__m_g", "xp4prema01_00", "xp4prema01_01", "c_xp4prema01_wasK__kwa_g", "c_xp4prema01_caSCAR__kwa_g", "c_xp4prema01_arSCARL__kwa_g", "xp4prema02_00", "xp4prema02_01", "c_xp4prema02_seq_waeM67_diw_g", "c_xp4prema02_wahLAT__kwa_g", "xp4prema03_00", "xp4prema03_01", "c_xp4prema03___tre_g", "c_xp4prema03___sre_g", "c_xp4prema03___th_g", "c_xp4prema03___qh_g", "xp4prema04_00", "xp4prema04_01", "c_xp4prema04_cXP4PR1__ga_g", "c_xp4prema04_cXP4PR2__ga_g", "c_xp4prema04___mk_g", "xp4prema05_00", "xp4prema05_01", "c_xp4prema05_pMP443S__kwa_g", "c_xp4prema05_smVAL__kwa_g", "xp4prema06_00", "xp4prema06_01", "c_xp4prema06_arM416__kwa_g", "c_xp4prema06___ak_g", "c_xp4prema06_cr44__ga_g", "xp4prema07_00", "xp4prema07_01", "c_xp4prema07_caACR__kwa_g", "c_xp4prema07_vA__ds_g", "c_xp4prema07_cr02__ga_g", "xp4prema08_00", "xp4prema08_01", "c_xp4prema08_srJNG90__kwa_g", "c_xp4prema08_seqUGS__spx_g", "c_xp4prema08_cr45__ga_g", "xp4prema09_00", "xp4prema09_01", "c_xp4prema09_mgM240__kwa_g", "c_xp4prema09___tr_g", "c_xp4prema09_cr03__ga_g", "xp4prema10_00", "xp4prema10_01", "c_xp4prema10_smPP19_as_kwa_g", "c_xp4prema10_smPP19_su_kwa_g", "c_xp4prema10_smPP19_en_kwa_g", "c_xp4prema10_smPP19_rc_kwa_g", "xp4ramb1_00", "xp4ramb1_01", "xp4ramb2_00", "xp4ramb2_01", "XP5ach05_00", "XP5ach05_01", "c_XP5ach05_vmA__ftx_g", "XP5ACH02_00", "XP5ACH02_01", "c_XP5ACH02___fct_g", "XP5ACH03_00", "XP5ACH03_01", "c_XP5ACH03_trKLR__kfx_g", "XP5ach04_00", "XP5ach04_01", "c_XP5ach04_vmA_trXP5_di_g", "XP5ACH01_00", "XP5ACH01_01", "c_XP5ACH01___pdk_g", "xp5ma01_00", "xp5ma01_01", "c_xp5ma01_vmA_trXP5_di_g", "xp5ma02_00", "xp5ma02_01", "c_xp5ma02___fr_g", "c_xp5ma02___fct_g", "c_xp5ma02_mwin_mgctf_roo_g", "xp5ma03_00", "xp5ma03_01", "c_xp5ma03___pdk_g", "c_xp5ma03_vmT__pdx_g", "c_xp5ma03_vIFV__pdx_g", "xp5ma04_00", "xp5ma04_01", "c_xp5ma04_vA__de_g", "c_xp5ma04_trKLR__rkv_g", "xp5ma05_00", "xp5ma05_01", "c_xp5ma05__whP_hsh_g", "xp5prema01_00", "xp5prema01_01", "c_xp5prema01_srM39__kwa_g", "c_xp5prema01___fck_g", "xp5prema02_00", "xp5prema02_01", "c_xp5prema02_arSCARL__kwa_g", "c_xp5prema02___fct_g", "xp5prema03_00", "xp5prema03_01", "c_xp5prema03_caHK53__kwa_g", "c_xp5prema03_vA__de_g", "xp5prema04_00", "xp5prema04_01", "c_xp5prema04_mgQBB95__kwa_g", "c_xp5prema04___rs_g", "xp5prema05_00", "xp5prema05_01", "c_xp5prema05_sgJackH_as_kwa_g", "c_xp5prema05_sgJackH_en_kwa_g", "c_xp5prema05_sgJackH_rc_kwa_g", "c_xp5prema05_sgJackH_su_kwa_g", "c_arM16__kwa_g", "c_arF2__kwa_g", "c_arM416__kwa_g", "c_arG3__kwa_g", "c_arFAMAS__kwa_g", "c_arL85A2__kwa_g", "c_arAUG__kwa_g", "c_arSCARL__kwa_g", "c_wahAUS__kwa_g", "c_arAEK__kwa_g", "c_arAK74__kwa_g", "c_arAN94__kwa_g", "c_arKH__kwa_g", "c_wahARU__kwa_g", "c_wahA__kwa_g", "c_srMK11__kwa_g", "c_srM39__kwa_g", "c_srHK417__kwa_g", "c_wahsrUSSemi__kwa_g", "c_srSVD__kwa_g", "c_srSKS__kwa_g", "c_srQBU88__kwa_g", "c_wahsrRUSemi__kwa_g", "c_srSV98__kwa_g", "c_srJNG90__kwa_g", "c_wahsrRUBolt__kwa_g", "c_srM98__kwa_g", "c_srM40__kwa_g", "c_srL96__kwa_g", "c_wahsrUSBolt__kwa_g", "c_wahSR__kwa_g", "c_mgRPK__kwa_g", "c_mgQBB95__kwa_g", "c_wahMRUClip__kwa_g", "c_mgM27__kwa_g", "c_mgMG36__kwa_g", "c_mgL86A1__kwa_g", "c_wahMUSClip__kwa_g", "c_mgPech__kwa_g", "c_mgT88__kwa_g", "c_wahMRUBelt__kwa_g", "c_mgM249__kwa_g", "c_mgM60__kwa_g", "c_mgM240__kwa_g", "c_mgLSAT__kwa_g", "c_wahMUSBelt__kwa_g", "c_wahM__kwa_g", "c_sg870__kwa_g", "c_sgM1014__kwa_g", "c_sgDAO__kwa_g", "c_sgJackH__kwa_g", "c_sgSPAS12__kwa_g", "c_wahsgUS__kwa_g", "c_sgSaiga__kwa_g", "c_sgUSAS__kwa_g", "c_wahsgRU__kwa_g", "c_wahSG__kwa_g", "c_smP90__kwa_g", "c_smUMP__kwa_g", "c_smM5K__kwa_g", "c_wahsmUS2__kwa_g", "c_smPP2000__kwa_g", "c_wahsmRU1__kwa_g", "c_smMP7__kwa_g", "c_smPDR__kwa_g", "c_wahsmUS1__kwa_g", "c_smVAL__kwa_g", "c_wahsmVAL__kwa_g", "c_smPP19__kwa_g", "c_wahsmRU2__kwa_g", "c_wahSM__kwa_g", "c_caAKS__kwa_g", "c_caA91__kwa_g", "c_caQBZ95B__kwa_g", "c_wahCRU__kwa_g", "c_caM4__kwa_g", "c_caSCAR__kwa_g", "c_caG36__kwa_g", "c_caSG553__kwa_g", "c_caHK53__kwa_g", "c_caMTAR21__kwa_g", "c_caACR__kwa_g", "c_wahCUS__kwa_g", "c_wahC__kwa_g", "c_wahUSG__kwa_g", "c_wahUGL__kwa_g", "c_wahU__kwa_g", "c_pM9__kwa_g", "c_pG17__kwa_g", "c_pg18__kwa_g", "c_pM9S__kwa_g", "c_pM93R__kwa_g", "c_pTaur__kwa_g", "c_pM412__kwa_g", "c_pMP443__kwa_g", "c_pMP443S__kwa_g", "c_pM1911__kwa_g", "c_pM1911S__kwa_g", "c_pM9F__kwa_g", "c_pTaurS__kwa_g", "c_pM1911T__kwa_g", "c_pM1911L__kwa_g", "c_pMP443L__kwa_g", "c_pG17S__kwa_g", "c_pg18S__kwa_g", "c_whP__kwa_g", "c_wLAAFIM__kwa_g", "c_wLAAIGL__kwa_g", "c_wahLAA__kwa_g", "c_wLATSMAW__kwa_g", "c_wLATRPG__kwa_g", "c_wLATJAV__kwa_g", "c_wahLAT__kwa_g", "c_wahL__kwa_g", "c_wasXBS__kwa_g", "c_wasXBK__kwa_g", "c_wasXB__kwa_g", "c_waH__kwa_g", "c_waeC4__kwa_g", "c_waeClay__kwa_g", "c_waeMine__kwa_g", "c_waeMort__kwa_g", "c_waeM67__kwa_g", "c_waE__kwa_g", "c_wasRT__kwa_g", "c_wasK__kwa_g", "c_wasDef__kwa_g", "c_wasM__kwa_g", "c_waS__kwa_g", "c_wA__kwa_g", "c_mbtM1A__ki_g", "c_mbtT90__ki_g", "c_vMBT__ki_g", "c_ifvBTR90__ki_g", "c_ifvLAV__ki_g", "c_ifvBMP__ki_g", "c_vIFV__ki_g", "c_vIFV__si_g", "c_aa9k22__ki_g", "c_aaLAV__ki_g", "c_vAA__ki_g", "c_mtdSPRUTSD__ki_g", "c_mtdM1128__ki_g", "c_vTD__ki_g", "c_martBM23__ki_g", "c_martHIM__ki_g", "c_vMART__ki_g", "c_vmL__ki_g", "c_jfF18__ki_g", "c_jfSu35__ki_g", "c_jfx1F35JSF__ki_g", "c_vJF__ki_g", "c_jaA10__ki_g", "c_jaSU25__ki_g", "c_vJA__ki_g", "c_vmaJ__ki_g", "c_ahMi28__ki_g", "c_ahAH1Z__ki_g", "c_vAH__ki_g", "c_shAH6__ki_g", "c_shz11__ki_g", "c_vSH__ki_g", "c_vmaH__ki_g", "c_vmaG__ki_g", "c_vmaD__ki_g", "c_vmA__ki_g", "c_trUH1__ki_g", "c_trAAV__ki_g", "c_trRIB__ki_g"
                , "c_trDpv__ki_g", "c_trHum__ki_g", "c_trVod__ki_g", "c_trITV__ki_g", "c_trKA60__ki_g", "c_trVDV__ki_g", "c_trSkid__ki_g", "c_trKLR__ki_g", "c_trQB__ki_g", "c_trVodnM__ki_g", "c_trVanM__ki_g", "c_trHmvM__ki_g", "c_trhmvnxp5__ki_g", "c_trVodnxp5__ki_g", "c_trXP5__ki_g", "c_vmT__ki_g", "c_vM__ki_g", "c_saaCRAM__ki_g", "c_saaPant__ki_g", "c_saa__ki_g", "c_atTOW__ki_g", "c_sKorn__ki_g", "c_sAT__ki_g", "c_sw__ki_g", "c_seqRad__ki_g", "c_seqEOD__ki_g", "c_seqUGS__ki_g", "c_seqMAV__ki_g", "c_seqM224__ki_g", "c_seqSOF__ki_g", "c_seq__ki_g", "c_eq__ki_g", "c_vA__ki_g", "c___hsd_ghva", "c___k_g", "c___d_g", "c_mwin__roo_g", "c_mlos__roo_g", "c___ka_g", "c_vA__de_g", "c___sfw_g", "c___shw_g", "c___k_ghvs", "c___deas_g", "c___sa_g", "c___nk_ghva", "c___dt_g", "c___ak_g", "c___sk_g", "c___nx_g", "c_fip__psx_g", "c_sep__psx_g", "c_thp__psx_g", "c___bc_g", "c___cd_g", "c___cdk_g", "c___ccp_g", "c___rs_g", "c___r_g", "c___h_g", "c___tad_g", "c___hsh_g", "c___re_g", "c_seqUGS_seqMAV___spx_g", "c___sua_g", "c_seqRad__sv_g", "c_seqMAV__spx_g", "c_waH__hsd_ghva", "c___cpd_g", "c___rk_g", "c___fct_g", "rank", "sc_general", "sc_team", "sc_bonus", "sc_squad", "sc_objective", "sc_award", "sc_support", "sc_assault", "sc_engineer", "sc_recon", "sc_specialkit", "sc_vehiclembt", "sc_vehicleifv", "sc_vehicleaa", "sc_vehicleah", "sc_vehiclesh", "sc_vehiclejet", "sc_vehiclelbt", "sc_vehicleart", "sc_unlock", "sc_coop", "elo", "elo_games", "spm", "kdr", "rs_time" };


        public static string GetStatName(int statNumber)
        {
            return stats[statNumber];
        }

        #region Loggers
        public static void LogRequest(Packet packet)
        {
            Log.Debug(string.Format("-> req: ID[{0}], {1}::{2} [0x{3}::0x{4}]",
                packet.msgNum.ToString(),
                GetComponentName((ushort)packet.componentId),
                GetCommandName((ushort)packet.componentId, (ushort)packet.commandId),
                ((ushort)packet.componentId).ToString("X4"),
                packet.commandId.ToString("X4")));
        }

        public static void LogResponse(Packet packet)
        {
            Log.Debug(string.Format("<- resp: ID[{0}], {1}::{2} [0x{3}::0x{4}]",
                packet.msgNum.ToString(),
                GetComponentName((ushort)packet.componentId),
                GetCommandName((ushort)packet.componentId, (ushort)packet.commandId),
                ((ushort)packet.componentId).ToString("X4"),
                packet.commandId.ToString("X4")));
        }

        public static void LogNotification(Packet packet)
        {
            Log.Debug(string.Format("<- async: ID[{0}], {1}::{2} [0x{3}::0x{4}]",
                packet.msgNum.ToString(),
                GetComponentName((ushort)packet.componentId),
                GetNotificationName((ushort)packet.componentId, (ushort)packet.commandId),
                ((ushort)packet.componentId).ToString("X4"),
                packet.commandId.ToString("X4")));
        }

        public static void LogUnhandledRequest(Packet packet)
        {
            Log.Warn(string.Format("unhandled: {0}::{1}", // unhandled: ID[{0}], {1}::{2} [0x{3}::0x{4}]
                GetComponentName((ushort)packet.componentId),
                GetCommandName((ushort)packet.componentId, (ushort)packet.commandId)/*,
                ((short)packet.componentId).ToString("X4"),
                packet.commandId.ToString("X4")*/));
        }
        #endregion

        public static void SendPacket(Packet packet, SslStream stream)
        {
            try
            {
                ProtoFire reply = new ProtoFire(packet);
                byte[] result = reply.Encode();

                if (packet.msgType == MessageType.NOTIFICATION)
                {
                    LogNotification(packet);
                }
                else
                {
                    LogResponse(packet);
                }

                stream.Write(result, 0, result.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static string GetComponentName(ushort componentId)
        {
            string result = "";

            switch (componentId)
            {
                case 0x1:
                    result = "AuthenticationComponent";
                    break;

                case 0x5:
                    result = "RedirectorComponent";
                    break;

                case 0x4:
                    result = "GameManager";
                    break;

                case 0x7:
                    result = "StatsComponent";
                    break;

                case 0x9:
                    result = "UtilComponent";
                    break;

                case 0xB:
                    result = "ClubsComponent";
                    break;

                case 0x801:
                    result = "RSPComponent";
                    break;

                case 0x7802:
                    result = "UserSessions";
                    break;

                case 0x1C:
                    result = "GameReporting";
                    break;

                default:
                    result = string.Format("0x" + componentId.ToString("X4"));
                    break;
            }

            return result;
        }

        public static string GetCommandName(ushort componentId, ushort commandId)
        {
            string result = "";

            switch ((Component)componentId)
            {
                case Component.AUTHENTICATION:
                    result = AuthenticationComponent.GetCommandName(commandId);
                    break;

                case Component.GAMEMANAGER:
                    result = GameManagerComponent.GetCommandName(commandId);
                    break;

                case Component.REDIRECTOR:
                    result = RedirectorComponent.GetCommandName(commandId);
                    break;

                case Component.STATS:
                    result = StatsComponent.GetCommandName(commandId);
                    break;

                case Component.UTIL:
                    result = UtilComponent.GetCommandName(commandId);
                    break;

                case Component.CLUBS:
                    result = ClubsComponent.GetCommandName(commandId);
                    break;

                case Component.RSP:
                    result = RSPComponent.GetCommandName(commandId);
                    break;

                case Component.USERSESSIONS:
                    result = UserSessionsComponent.GetCommandName(commandId);
                    break;

                case Component.GAMEREPORTING:
                    result = GameReportingComponent.GetCommandName(commandId);
                    break;

                default:
                    result = string.Format("0x" + commandId.ToString("X4"));
                    break;
            }

            return result;
        }

        public static string GetNotificationName(ushort componentId, ushort notificationId)
        {
            string result = "";

            switch ((Component)componentId)
            {
                case Component.GAMEMANAGER:
                    result = GameManagerComponent.GetNotificationName(notificationId);
                    break;

                case Component.STATS:
                    result = StatsComponent.GetNotificationName(notificationId);
                    break;

                case Component.USERSESSIONS:
                    result = UserSessionsComponent.GetNotificationName(notificationId);
                    break;

                case Component.GAMEREPORTING:
                    result = GameReportingComponent.GetNotificationName(notificationId);
                    break;

                default:
                    result = string.Format("0x" + notificationId.ToString("X4"));
                    break;
            }

            return result;
        }
    }
}