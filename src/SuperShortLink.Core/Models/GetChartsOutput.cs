namespace SuperShortLink.Charts
{
    public class GetChartsOutput
    {
        public GetChartsOutput(int length)
        {
            Access = new int[length];
            Generate = new int[length];
        }

        public int[] Access { get; set; }

        public int[] Generate { get; set; }
    }
}
