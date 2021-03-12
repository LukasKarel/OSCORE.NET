using System;
using System.Collections.Generic;
using System.Text;
using WorldDirect.CoAP.Common;

namespace WorldDirect.Oscore.Option
{
    public struct OscoreOptionValue
    {
        public UInt3 Length { get; set; }

        public bool KeyIdFlag { get; set; }

        public override string ToString() => $"{Length} - {KeyIdFlag}";
    }
}
