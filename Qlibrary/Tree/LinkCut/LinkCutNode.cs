namespace Qlibrary
{
    public class LinkCutNode<T>
    {
        public LinkCutNode<T> Left { get; set; }
        public LinkCutNode<T> Right { get; set; }
        public LinkCutNode<T> Parent { get; set; }
        public T Key { get; set; }
        public T Sum { get; set; }
        public int Count { get; set; }
        public bool Reversed { get; set; }

        public LinkCutNode(T key)
        {
            Key = key;
            Sum = key;
            Count = 1;
            Reversed = false;
        }
    }
}