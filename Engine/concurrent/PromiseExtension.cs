using System.Collections.Generic;
using UnityEngine;

namespace CofyEngine
{
    public static class PromiseExtension
    {
        public static PromiseImpl<List<T>> Sequence<T>(this List<PromiseImpl<T>> listOfPromise)
        {
            PromiseImpl<List<T>> sequence = new PromiseImpl<List<T>>(() =>
            {
                float sum = 0;
                for (var i = 0; i < listOfPromise.Count; i++)
                {
                    var promise = listOfPromise[i];
                    sum += promise.progresFunc();
                }

                return sum / listOfPromise.Count;
            });

            List<T> result = new List<T>();

            for (var i = 0; i < listOfPromise.Count; i++)
            {
                var promise = listOfPromise[i];
                promise.Completed += validation =>
                {
                    if (validation.hasException)
                    {
                        sequence.Reject(validation.target.ex);
                    }
                    else
                    {
                        result.Add(validation.target.result);
                        if (listOfPromise.TrueForAll(
                                p => p.isDone && Mathf.Approximately(p.progresFunc(), 1)))
                        {
                            sequence.Resolve(result);
                        }
                    }
                };
            }

            return sequence;
        }
    }
}