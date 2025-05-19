using BBE.CustomClasses;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBE.Creators
{
    class PostersCreator
    {
        public static void AddPosterToFloors(PosterObject poster, int F1, int F2, int F3, int F4, int F5, int END)
        {

            if (F1 > 0)
                FloorData.Get("F1").posters.Add(new WeightedPosterObject() { selection = poster, weight = F1 });
            if (F2 > 0)
                FloorData.Get("F2").posters.Add(new WeightedPosterObject() { selection = poster, weight = F2 });
            if (F3 > 0)
                FloorData.Get("F3").posters.Add(new WeightedPosterObject() { selection = poster, weight = F3 });
            if (F4 > 0)
                FloorData.Get("F4").posters.Add(new WeightedPosterObject() { selection = poster, weight = F4 });
            if (F5 > 0)
                FloorData.Get("F5").posters.Add(new WeightedPosterObject() { selection = poster, weight = F5 });
            if (END > 0)
                FloorData.Get("END").posters.Add(new WeightedPosterObject() { selection = poster, weight = END });
        }
        private static void AddPosterToFloors(PosterObject poster, int F1, int F2, int F3, int END) => AddPosterToFloors(poster, F1, F2, F3, F2, F3, END);

        public static void Create()
        {
        }
    }
}
