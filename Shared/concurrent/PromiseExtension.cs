﻿using System.Collections.Generic;
using UnityEngine;

public static class PromiseExtension
{
    public static Future<List<T>> Group<T>(this List<Future<T>> listOfPromise)
    {
        Promise<List<T>> sequence = new Promise<List<T>>(() =>
        {
            float sum = 0;
            for (var i = 0; i < listOfPromise.Count; i++)
            {
                var promise = listOfPromise[i].promise;
                if (promise.isFailure) continue;
                sum += promise.progressFunc();
            }

            return sum / listOfPromise.Count;
        });

        List<T> result = new List<T>();

        for (var i = 0; i < listOfPromise.Count; i++)
        {
            var promise = listOfPromise[i].promise;
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
                            p => p.promise.isCompleted && Mathf.Approximately(p.promise.progressFunc(), 1)))
                    {
                        sequence.Resolve(result);
                    }
                }
            };
        }

        return sequence.future;
    }
}