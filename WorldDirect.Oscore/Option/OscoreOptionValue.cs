using System;
using System.Collections.Generic;
using System.Text;
using WorldDirect.CoAP.Common;

namespace WorldDirect.Oscore.Option
{
    public struct OscoreOptionValue
    {
        public ReadOnlyMemory<byte> PartialIV;

        public ReadOnlyMemory<byte> KidContext;

        public ReadOnlyMemory<byte> Kid;


        //public override string ToString() => $"{Length} - {KeyIdFlag}";
    }
}
