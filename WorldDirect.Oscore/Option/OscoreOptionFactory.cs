using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldDirect.CoAP;
using WorldDirect.CoAP.Common;
using WorldDirect.CoAP.V1.Options;

namespace WorldDirect.Oscore.Option
{
    public class OscoreOptionFactory : IOptionFactory
    {
        public int Number => OscoreOption.NUMBER;

        public CoapOption Create(OptionData src)
        {
            OscoreOptionValue optionValue;
            optionValue.Kid = ReadOnlyMemory<byte>.Empty;
            optionValue.KidContext = ReadOnlyMemory<byte>.Empty;
            optionValue.PartialIV = ReadOnlyMemory<byte>.Empty;
            var decode = src.Value.Reverse().ToArray();

            if (decode.Length > 0)
            {
                byte oscoreFlagBits = decode[0];
                if((oscoreFlagBits & 0xE0) != 0) 
                {
                    // these bits must be set to zero -> otherwise message is malformed
                    return null;
                }
                bool kidContextPresent = (oscoreFlagBits & 0x10) != 0 ? true : false;
                bool kidPresent = (oscoreFlagBits & 0x08) != 0 ? true : false;
                UInt3 n = (UInt3)(oscoreFlagBits & 0x07);
                if(n >= 6) 
                {
                    // reserved values
                    return null;
                }
                var minimumLength = 1 + n;
                if (kidContextPresent)
                {
                    // one byte length of kid context
                    minimumLength++;
                }
                if(kidPresent)
                {
                    minimumLength++;
                }
                if(decode.Length < minimumLength)
                {
                    // malformed option value
                    return null;
                }

                optionValue.PartialIV = decode.AsMemory(1,n);
                var s = 0;
                if (kidContextPresent)
                {
                    s = decode[1 + n];
                    if (decode.Length < 2 + n + s) 
                    {
                        // kid context is not encoded
                        return null;
                    }
                    if(s > 0)
                    {
                        optionValue.KidContext = decode.AsMemory(2 + n, s);
                    }
                    s++;
                }
                if(kidPresent)
                {
                    if(decode.Length <= n + 1 + s) 
                    {
                        // kid should be encoded but there is no space left.
                        return null;
                    }
                    optionValue.Kid = decode.AsMemory(n + 1 + s);
                }
                else
                {
                    if(decode.Length > 1 + n + s)
                    {
                        // kid encoded but not defined
                        return null;
                    }
                }
                
            }
            

            return new OscoreOption(optionValue, src.Value);
        }
    }
}
