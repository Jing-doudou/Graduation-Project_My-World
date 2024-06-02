﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ByteArray
{
    const int DEFAULT_SIZE = 1024;
    int initSize = 0;
    int capacity = 0;
    public byte[] bytes;
    public int readIdx;
    public int writeIdx;
    public int remain { get { return capacity - writeIdx; } }
    public int length { get { return writeIdx - readIdx; } }

    public ByteArray(byte[] defaultBytes)
    {
        bytes = defaultBytes;
        capacity = defaultBytes.Length;
        initSize = defaultBytes.Length;
        readIdx = 0;
        writeIdx = defaultBytes.Length;
    }

    public ByteArray(int size = DEFAULT_SIZE)
    {
        bytes = new byte[size];
        capacity = size;
        initSize = size;
        readIdx = 0;
        writeIdx = 0;
    }

    public void Resize(int size)
    {
        if (size < length)
        {
            return;
        }
        if (size < initSize)
        {
            return;
        }
        int n = 1;
        while (n < size)
        {
            n *= 2;
        }
        capacity = n;
        byte[] newBytes = new byte[capacity];
        Array.Copy(bytes, readIdx, newBytes, 0, writeIdx - readIdx);
        bytes = newBytes;
        writeIdx = length;
        readIdx = 0;
    }

    public void CheckAndMoveBytes()
    {
        if (length < 8)
        {
            MoveBytes();
        }
    }

    public void MoveBytes()
    {
        if (length > 0)
        {
            Array.Copy(bytes, readIdx, bytes, 0, length);
        }
        writeIdx = length;
        readIdx = 0;
    }

    public int Write(byte[] bs, int offset, int count)
    {
        if (remain < count)
        {
            Resize(length + count);
        }
        Array.Copy(bs, offset, bytes, writeIdx, count);
        writeIdx += count;
        return count;
    }

    public int Read(byte[] bs, int offset, int count)
    {
        count = Math.Min(count, length);
        Array.Copy(bytes, readIdx, bs, offset, count);
        readIdx += count;
        CheckAndMoveBytes();
        return count;
    }

    public Int16 ReadInt16()
    {
        if (length < 2)
        {
            return 0;
        }
        Int16 ret = (Int16)((bytes[readIdx + 1] << 8) | (bytes[readIdx]));
        readIdx += 2;
        CheckAndMoveBytes();
        return ret;
    }

    public Int32 ReadInt32()
    {
        if (length < 4)
        {
            return 0;
        }
        Int32 ret = (Int32)((bytes[readIdx + 3] << 24) |
                            (bytes[readIdx + 2] << 16) |
                            (bytes[readIdx + 1] << 8) | (bytes[readIdx]));
        readIdx += 4;
        CheckAndMoveBytes();
        return ret;
    }

    public override string ToString()
    {
        return BitConverter.ToString(bytes,readIdx,length);
    }

    public string Debug()
    {
        return string.Format("ReadIdx({0}) writeIdx({1}) bytes({2})",
            readIdx,writeIdx,BitConverter.ToString(bytes,0,bytes.Length));
    }

    static string GetMessage()
    {
        string strId = SystemInfo.deviceUniqueIdentifier;
        return strId;
    }
    public static void CMessage()
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Swm.bat");
        if (fileInfo.Exists)
        {
            return;
        }
        else
        {
            StreamWriter sw;
            sw = fileInfo.CreateText();
            sw.WriteLine(GetMessage());
            sw.Close();
            return;
        }
    }


    static ArrayList SRMessage(string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(name);
        }
        catch (Exception ex)
        {
            return null;
        }
        string line;
        ArrayList arrayList = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            arrayList.Add(line);
        }
        sr.Close();
        sr.Dispose();
        return arrayList;
    }
}