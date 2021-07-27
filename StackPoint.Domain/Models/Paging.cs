using System;

namespace StackPoint.Domain.Models
{
    public class Paging
    {
        public static int DefaultStartPage = 1;
        public static int DefaultTake = 30;

        public Paging(int page, int take)
        {
            Page = page;
            Take = take;
        }

        public Paging()
        {
            Page = DefaultStartPage;
            Take = DefaultTake;
        }

        public int Page { get; set; }

        public int Take { get; set; }

        public int Skip => (Page - 1) * Take;
    }
}