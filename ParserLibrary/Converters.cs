/******************************************************************
 * File: Converters.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Data.HashFunction.xxHash;
using System.Security.Cryptography;
using System.Text;

namespace ParserLibrary;

public class Hash:HashOutput
{
    public int SizeInBits = 64;
    public Hash()
    {
        hashConverter = new Hasher();
    }
}

public class HashOutput : ConverterOutput
{
    public AliasProducer aliasProducer { get; set; }
    public HashConverter hashConverter { get; set; }

    public class OutItem
    {
        public string hash { get; set; }
        public string alias { get; set; }
    }

    public AbstrParser.UniEl ConvertToNew(AbstrParser.UniEl el)
    {
        var value = el.Value.ToString();
        AbstrParser.UniEl result = new AbstrParser.UniEl() { Name = el.Name };
        if (aliasProducer == null)
        {
            result.Value = hashConverter.hash(value);
            return result;
        }
        else
        {
            var el1 = new AbstrParser.UniEl(result) { Name = "h", Value = hashConverter.hash(value) };
            var el2 = new AbstrParser.UniEl(result) { Name = "a", Value = aliasProducer.getAlias(value) };
            return result;
        }
        //            return JsonSerializer.Serialize<OutItem>(new OutItem() { alias = aliasProducer.getAlias(value), hash = hashConverter.hash(value) });   
    }

    public override AbstrParser.UniEl Convert(string value, AbstrParser.UniEl input_el, AbstrParser.UniEl output_el)
    {
        if (aliasProducer == null)
        {
            output_el.Value = hashConverter.hash(value);
            return output_el;
        }
        else
        {
            var el1 = new AbstrParser.UniEl(output_el) { Name = "h", Value = hashConverter.hash(value) };
            var el2 = new AbstrParser.UniEl(output_el) { Name = "a", Value = aliasProducer.getAlias(value) };
            return output_el;
        }
//            return JsonSerializer.Serialize<OutItem>(new OutItem() { alias = aliasProducer.getAlias(value), hash = hashConverter.hash(value) });   
    }
}

public abstract class ConverterOutput
{
    public abstract AbstrParser.UniEl Convert(string value, AbstrParser.UniEl input_el, AbstrParser.UniEl output_el);
}

public class Hasher : HashConverter
{
    public int SizeInBits = 64;
    bool init = false;

    public Hasher()
    {
    }

    IxxHash instance;

    void Init()
    {
        if (!init)
        {
            init = true;
            instance = xxHashFactory.Instance.Create(new xxHashConfig() { HashSizeInBits = SizeInBits });
        }
    }

    public override string hash(string value)
    {
        Init();
        return instance.ComputeHash(Encoding.ASCII.GetBytes(value)).AsHexString();
    }
}

public class CryptoHash : HashConverter
{
    static public string pwd = "QWE123";

    public override string hash(string value)
    {
        var data = Encoding.UTF8.GetBytes(pwd + value);
//            string sHash;
        using (SHA256 shaM = new SHA256Managed())
        {
            byte[] hash = shaM.ComputeHash(data);
            return Convert.ToBase64String(hash);
//                Console.WriteLine(sHash);
        }
        //          throw new NotImplementedException();
    }
}

public abstract class HashConverter
{
    public abstract string hash(string value);
}