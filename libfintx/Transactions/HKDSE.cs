﻿/*	
 * 	
 *  This file is part of libfintx.
 *  
 *  Copyright (c) 2016 - 2017 Torsten Klinger
 * 	E-Mail: torsten.klinger@googlemail.com
 * 	
 * 	libfintx is free software; you can redistribute it and/or
 *	modify it under the terms of the GNU Lesser General Public
 * 	License as published by the Free Software Foundation; either
 * 	version 2.1 of the License, or (at your option) any later version.
 *	
 * 	libfintx is distributed in the hope that it will be useful,
 * 	but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * 	Lesser General Public License for more details.
 *	
 * 	You should have received a copy of the GNU Lesser General Public
 * 	License along with libfintx; if not, write to the Free Software
 * 	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 * 	
 */

namespace libfintx
{
    public static class HKDSE
    {
        /// <summary>
        /// Collect
        /// </summary>
        public static string Init_HKDSE(int BLZ, string Accountholder, string AccountholderIBAN, string AccountholderBIC,
            string Payer, string PayerIBAN, string PayerBIC, decimal Amount, string Usage, string SettlementDate, string MandateNumber,
            string MandateDate, string CeditorIDNumber, string URL, int HBCIVersion, string UserID, string PIN)
        {
            Log.Write("Starting job HKDSE: Collect money");

            string segments = "HKDSE:" + SEGNUM.SETVal(3) + ":1+" + AccountholderIBAN + ":" + AccountholderBIC + "+urn?:iso?:std?:iso?:20022?:tech?:xsd?:pain.008.002.02+@@";

            var message = pain00800202.Create(Accountholder, AccountholderIBAN, AccountholderBIC, Payer, PayerIBAN, PayerBIC, Amount, Usage, SettlementDate, MandateNumber, MandateDate, CeditorIDNumber);

            segments = segments.Replace("@@", "@" + (message.Length - 1) + "@") + message;

            segments = segments + "HKTAN:" + SEGNUM.SETVal(4) + ":" + Segment.HITANS + "'";

            SEG.NUM = SEGNUM.SETInt(4);

            var TAN = FinTSMessage.Send(URL, FinTSMessage.Create(HBCIVersion, Segment.HNHBS, Segment.HNHBK, BLZ, UserID, PIN, Segment.HISYN, segments, Segment.HIRMS, SEG.NUM));

            Segment.HITAN = Helper.Parse_String(Helper.Parse_String(TAN, "HITAN", "'").Replace("?+", "??"), "++", "+").Replace("??", "?+");

            Helper.Parse_Message(TAN);

            return TAN;
        }
    }
}
