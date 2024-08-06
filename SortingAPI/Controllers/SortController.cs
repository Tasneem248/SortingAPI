using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SortingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortController : ControllerBase
    {
        [HttpPost]
        public IActionResult SortNumbers([FromBody] int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                return BadRequest("Numbers array cannot be null or empty.");
            }

            var sortingResults = new Dictionary<string, double>();

            sortingResults.Add("BubbleSort", MeasureSortTime(BubbleSort, numbers));
            sortingResults.Add("InsertionSort", MeasureSortTime(InsertionSort, numbers));
            sortingResults.Add("QuickSort", MeasureSortTime(QuickSort, numbers));
            sortingResults.Add("MergeSort", MeasureSortTime(MergeSort, numbers));
            sortingResults.Add("SelectionSort", MeasureSortTime(SelectionSort, numbers));

            return Ok(sortingResults);
        }

        private double MeasureSortTime(Action<int[]> sortMethod, int[] numbers)
        {
            int[] numbersCopy = (int[])numbers.Clone();
            var stopwatch = Stopwatch.StartNew();
            sortMethod(numbersCopy);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        private void BubbleSort(int[] numbers)
        {
            int n = numbers.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        var temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;
                    }
                }
            }
        }

        private void InsertionSort(int[] numbers)
        {
            int n = numbers.Length;
            for (int i = 1; i < n; i++)
            {
                int key = numbers[i];
                int j = i - 1;
                while (j >= 0 && numbers[j] > key)
                {
                    numbers[j + 1] = numbers[j];
                    j--;
                }
                numbers[j + 1] = key;
            }
        }

        private void QuickSort(int[] numbers)
        {
            QuickSort(numbers, 0, numbers.Length - 1);
        }

        private void QuickSort(int[] numbers, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(numbers, low, high);
                QuickSort(numbers, low, pi - 1);
                QuickSort(numbers, pi + 1, high);
            }
        }

        private int Partition(int[] numbers, int low, int high)
        {
            int pivot = numbers[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (numbers[j] < pivot)
                {
                    i++;
                    var temp = numbers[i];
                    numbers[i] = numbers[j];
                    numbers[j] = temp;
                }
            }
            var temp1 = numbers[i + 1];
            numbers[i + 1] = numbers[high];
            numbers[high] = temp1;
            return i + 1;
        }

        private void MergeSort(int[] numbers)
        {
            if (numbers.Length <= 1) return;
            int mid = numbers.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[numbers.Length - mid];

            Array.Copy(numbers, 0, left, 0, mid);
            Array.Copy(numbers, mid, right, 0, numbers.Length - mid);

            MergeSort(left);
            MergeSort(right);
            Merge(numbers, left, right);
        }

        private void Merge(int[] numbers, int[] left, int[] right)
        {
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
            {
                if (left[i] <= right[j])
                {
                    numbers[k++] = left[i++];
                }
                else
                {
                    numbers[k++] = right[j++];
                }
            }
            while (i < left.Length)
            {
                numbers[k++] = left[i++];
            }
            while (j < right.Length)
            {
                numbers[k++] = right[j++];
            }
        }

        private void SelectionSort(int[] numbers)
        {
            int n = numbers.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (numbers[j] < numbers[minIdx])
                    {
                        minIdx = j;
                    }
                }
                var temp = numbers[minIdx];
                numbers[minIdx] = numbers[i];
                numbers[i] = temp;
            }
        }
    }
}
