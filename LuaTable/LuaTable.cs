﻿

namespace LuaTable


        #region 内部类

        public class LuaTablePairs : IEnumerator, IEnumerable, IDisposable
        {
            private LuaTable table;
            private int index = 0;
            public LuaTablePairs(LuaTable t)
            {
                this.table = t;
            }

            public object Current
            {
                get
                {
                    if (table == null)
                    {
                        return null;
                    }
                    return table[index];
                }
            }

            public void Dispose()
            {
                index = 0;
            }

            public IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }

            public bool MoveNext()
            {
                if (table == null)
                {
                    return false;
                }

                return table[++index] != null;
            }

            public void Reset()
            {
                index = 0;
            }
        }

        public class LuaTableIpairs : IEnumerator, IEnumerable, IDisposable
        {
            private LuaTable table;
            private LuaValue preKey = null;
            public LuaTableIpairs(LuaTable t)
            {
                this.table = t;
            }

            public object Current
            {
                get
                {
                    if (table == null)
                    {
                        return null;
                    }
                    return table[preKey.Value];
                }
            }

            public void Dispose()
            {
                this.preKey = null;
            }

            public IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }

            public bool MoveNext()
            {
                if(table == null)
                {
                    return false;
                }

                int i = FindIndex(preKey);

                for (; i < table.sizeArray; ++i)
                {

                    if (table.array[i].Value != null)
                    {
                        this.preKey = new LuaValue(i + 1);
                        return true;
                    }
                }
                int sizeNode = table.nodes == null ? 0 : (1 << table.lsizeNode);
                for (i -= table.sizeArray; i < sizeNode; i++)
                {
                    if(!table.NodeIsDummy(table.nodes[i]))
                    {
                        this.preKey = table.nodes[i].Key.KeyValue;
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                this.preKey = null;
            }

            private int ArrayIndex(LuaValue key)
            {
                if (key.GetValueType() == typeof(int))
                {
                    return (int)key;
                }
                return 0;
            }
            private int FindIndex(LuaValue key)
            {
                int i;
                if (key == null || key.Value == null)
                {
                    return 0;
                }

                i = this.ArrayIndex(key);

                if (i != 0 && i <= table.sizeArray)
                {
                    return i;
                }
                else
                {
                    int nx;
                    int n = table.MainPosition(key);
                    for (;;)
                    {
                        if (table.nodes[n].Key.KeyValue.Value == key.Value)
                        {
                            return (n + 1) + table.sizeArray;
                        }
                        nx = table.nodes[n].Key.next;
                        if (nx == 0)
                        {
                            throw new Exception("invalid key to 'next'");
                        }
                        else n += nx;
                    }
                }
            }

        }






        #endregion
        #region 支持LuaValue


        #endregion

        private byte lsizeNode;

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            int j = this.sizeArray;
            if (j > 0 && this.array[j - 1].Value == null)
            {
                /* 二分搜索 */
                int i = 0;
                while (j - i > 1)
                {
                    int m = (i + j) / 2;
                    if (this.array[m - 1].Value == null) j = m;
                    else i = m;
                }
                return i;
            }
            else if (j == 0) 
                return j;
            else return UnboundSearch(j);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        public void Insert(object obj)
        {
            int length = this.GetLength();
            this[length + 1] = obj;
        }

        public void Insert(object obj, int pos)
        {
            int length = this.GetLength();
            if(pos >= 1 && pos <= length)
            {
                for(int i = length + 1; i > pos; --i)
                {
                    this[i] = this[i - 1];
                }
                this[pos] = obj;
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public object Remove(int pos)
        {
            int length = this.GetLength();
            if (pos >= 1 && pos <= length)
            {
                object val = this[pos];
                for(; pos < length; ++ pos)
                {
                    this[pos] = this[pos + 1];
                }
                return val;
            }
            return null;
        }

        private LuaTablePairs pairs;
        private LuaTableIpairs ipairs;
        public static LuaTablePairs Pairs(LuaTable table)
        {
            if(table == null)
            {
                return null;
            }
            return table.pairs;
        }

        public static LuaTableIpairs Ipairs(LuaTable table)
        {
            if (table == null)
            {
                return null;
            }
            return table.ipairs;
        }




        private int UnboundSearch(int j)
        {
            int i = j;
            j++;
            while (this.GetIntValue(j) != null)
            {
                i = j;
                if (j > int.MaxValue / 2)
                {
                    i = 1;
                    while (this.GetIntValue(i) != null) i++;
                    return i - 1;
                }
                j *= 2;
            }
            while(j - i > 1)
            {
                int m = (i + j) / 2;
                if (this.GetIntValue(m) == null) j = m;
                else i = m;
            }
            return i;
        }

        /// <summary>
            //拷贝之前的元素
            for (int i = 0; ; ++i)

        /// <summary>

        /// <summary>

        /// <summary>

        /// <summary>

        /// <summary>

            mp = this.nodes == null ? null : this.nodes[nIdx];

                if (fIdx == -1)

                    //换位置
                    this.nodes[fIdx] = mp;

        /// <summary>

        /// <summary>

        /// <summary>

        /// <summary>

        /// <summary>

        /// <summary>


        /// <summary>


        private bool NodeIsDummy(TableNode node)
        {
            if(node == null || node.Val == null || node.Val.Value == null)
            {
                return true;
            }
            return false;
        }


        /// <summary>

        /// <summary>

        /// <summary>
                        na = a;

        /// <summary>
            if (nasize > oldasize) //array需要增长
            {
                    /* 因为上面已经修改了数组大小，所以下面的插入一定是插入的hash */
                    if (this.array[i] != null)
                /* 收缩数组大小 */
                LuaValue[] temp = new LuaValue[nasize];
            /* 重新插入到hash */
            for (int i = 0; i < oldhsize; ++i)