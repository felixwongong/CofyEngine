﻿using System.Collections.Generic;
using UnityEngine;

public static class PromiseExtension
{
    public static Future<List<T>> Group<T>(this List<Future<T>> futures)
    {
        Promise<List<T>> sequence = new Promise<List<T>>(() =>
        {
            float sum = 0;
            for (var i = 0; i < futures.Count; i++)
            {
                var future = futures[i];
                if (future.isFailure) continue;
                sum += future.progress;
            }

            return sum / futures.Count;
        });

        List<T> result = new List<T>();

        for (var i = 0; i < futures.Count; i++)
        {
            var future = futures[i];

            future.OnCompleted(validation =>
            {
                if (validation.hasException)
                {
                    sequence.Reject(validation.target.ex);
                }
                else
                {
                    result.Add(validation.target.result);
                    if (futures.TrueForAll(
                            f => f.isCompleted && Mathf.Approximately(f.progress, 1)))
                    {
                        sequence.Resolve(result);
                    }
                }
            });
        }

        return sequence.future;
    }
}