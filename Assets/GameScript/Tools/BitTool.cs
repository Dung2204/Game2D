using System.Collections.Generic;

//位操作工具
public class BitTool
{
    /// <summary>
    /// 位设置
    /// </summary>
    /// <param name="num">检测数字</param>
    /// <param name="pos">第几位，从1开始</param>
    /// <returns></returns>
    public static int BitSet(int num,ushort pos)
    {
        if (pos <= 0 || pos > 32)
        {
            return num;
        }
        return num | (0x00000001 << (pos - 1));
    }

    /// <summary>
    /// 位清理
    /// </summary>
    /// <param name="num">检测数字</param>
    /// <param name="pos">第几位，从1开始</param>
    /// <returns></returns>
    public static int BitClear(int num, ushort pos)
    {
        if (pos <= 0 || pos > 32)
        {
            return num;
        }
        return num & (~(0x00000001 << (pos - 1)));
    }

    /// <summary>
    /// 位检测
    /// </summary>
    /// <param name="num">检测数字</param>
    /// <param name="pos">第几位，从1开始</param>
    /// <returns></returns>
    public static bool BitTest(int num, ushort pos)
    {
        if (pos <= 0 || pos > 32)
        {
            return false;
        }
        return ((num >> (pos - 1)) & 0x00000001) == 1;
    }

    /// <summary>
    /// 获取一个十进制数位为1的索引数组 如num = 5,则返回[1,3]
    /// </summary>
    /// <param name="num">数字</param>
    /// <returns></returns>
    public static int[] GetIndexOfBit1(uint num) {
        List<int> indexs = new List<int>();
	    if (num == 0) {
            return indexs.ToArray();
        }
        int index = 0;
	    for (uint q = num; q > 0; q = q / 2 ){
            uint m = q % 2;
            index++;
		    if (m == 1){
                indexs.Add(index);
            }
	    }
	    return indexs.ToArray();
    }
}
