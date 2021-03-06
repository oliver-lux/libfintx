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
    public static class HKPPD
    {
        /// <summary>
        /// Load prepaid
        /// </summary>
        public static string Init_HKPPD(int BLZ, string IBAN, string BIC, int MobileServiceProvider, string PhoneNumber, int Amount,
            string URL, int HBCIVersion, string UserID, string PIN)
        {
            Log.Write("Starting job HKPPD: Load prepaid");

            string segments = "HKPPD:" + SEGNUM.SETVal(3) + ":2+" + IBAN + ":" + BIC + "+" + MobileServiceProvider + "+" + PhoneNumber + "+" + Amount + ",:EUR'";

            segments = segments + "HKTAN:" + SEGNUM.SETVal(4) + ":" + Segment.HITANS + "'";

            SEG.NUM = SEGNUM.SETInt(4);

            var TAN = FinTSMessage.Send(URL, FinTSMessage.Create(HBCIVersion, Segment.HNHBS, Segment.HNHBK, BLZ, UserID, PIN, Segment.HISYN, segments, Segment.HIRMS, SEG.NUM));

            Segment.HITAN = Helper.Parse_String(Helper.Parse_String(TAN, "HITAN", "'").Replace("?+", "??"), "++", "+").Replace("??", "?+");

            Helper.Parse_Message(TAN);

            return TAN;
        }
    }
}
