using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EasyOpen.Utils
{
    public static class BListView
    {
        public static ListViewItem GetItem(this ListView.ListViewItemCollection list, string name)
        {
            foreach (ListViewItem item in list)
            {
                if (item.Text == name)
                {
                    ListViewItem view = item.Clone() as ListViewItem;
                    return view;
                }
            }
            return null;
        }


        public static List<ListViewItem> ToList(this ListView.ListViewItemCollection list)
        {
            List<ListViewItem> listItem = new List<ListViewItem>();
            foreach (ListViewItem item in list)
            {
                ListViewItem view = item.Clone() as ListViewItem;
                listItem.Add(view);

            }
            return listItem;
        }

        public static T FindOrDefault<T>(this IEnumerable<T> source, Predicate<T> match)
        {
            if (source == null)
            {
                throw new Exception("无法计算空值");
            }
            List<T> list = source.ToList();
            T t = default(T);
            var l = list.FindAll(match);
            if (l != null && l.Count > 0)
            {
                t = l[0];
            }
            return t;
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new Exception("无法计算空值");
            }
            return new List<TSource>(source);
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new Exception("无法计算空值");
            }
            return source.ToList().ToArray();
        }
    }
}
