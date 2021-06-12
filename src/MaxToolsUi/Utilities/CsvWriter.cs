using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using MaxToolsUi.Models;
using System.Windows.Forms;

namespace MaxToolsUi.Utilities
{
    public static class CsvWriter
    {
        private static string _lastCsvFile = null;

        public static SaveFileDialog CreateSaveCsvDialog()
        {
            var dialog = new SaveFileDialog();

            if (string.IsNullOrEmpty(_lastCsvFile))
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                dialog.FileName = _lastCsvFile;
            }

            dialog.OverwritePrompt = true;
            dialog.Filter = @"CSV files (*.csv)|*.csv";

            return dialog;
        }

        public static IEnumerable<dynamic> GetNodeModelRecords(this IEnumerable<NodeModel> nodeModels)
        {
            var nodeModelList = nodeModels.ToList();

            var propertyNames = nodeModelList
                .SelectMany(n => n.Properties)
                .Select(p => p.Name)
                .OrderBy(v => v)
                .ToArray();

            var records = nodeModelList.Select(n =>
            {
                var dict = new Dictionary<string, object>();
                dict["Node_Name"] = n.Name;
                foreach (var pn in propertyNames)
                {
                    dict[pn] = n.Properties.FirstOrDefault(p => p.Name == pn)?.Value ?? "";
                }

                // Convert the dictionary into a dynamic object.
                return dict.Aggregate(
                    new ExpandoObject() as IDictionary<string, object>,
                    (acc, cur) =>
                    {
                        acc.Add(cur.Key, cur.Value);
                        return acc;
                    });
            });

            return records;
        }

        public static void WriteToCsv(this IEnumerable<NodeModel> nodeModels, string filePath)
        {
            FSUtilities.CreateFileDirectory(filePath);
            using (var writer = File.CreateText(filePath))
            {
                var csvWriter = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);

                var records = nodeModels.GetNodeModelRecords();
                csvWriter.WriteRecords(records);
            }
        }

        public static void WriteToCsvWithDialog(this IEnumerable<NodeModel> models)
        {
            var dialog = CreateSaveCsvDialog();
            var result = dialog.ShowDialog();

            if (result != DialogResult.OK)
                return;

            models.WriteToCsv(dialog.FileName);
            _lastCsvFile = dialog.FileName;
        }
    }
}
