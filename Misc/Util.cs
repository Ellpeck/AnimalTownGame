using System;

namespace AnimalTownGame.Misc {
    public static class Util {

        public static T[] GetEnum<T>() {
            return (T[]) Enum.GetValues(typeof(T));
        }

    }
}