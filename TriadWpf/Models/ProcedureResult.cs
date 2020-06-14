using System.Collections.Generic;

namespace TriadWpf.Models
{
    public class ProcedureResult
    {
        public string ProcedureName { get; set; }

        /// <summary>
        /// Это значение, которое может возвращать процедура, как функция.
        /// Может быть только простым типом (integer, real и т.д.).
        /// На triad выглядит так:
        /// infprocedure ИмяПроцедуры(event ev; in array [10,10] of integer var) : integer
        /// Случае в верху integer - значение, которое возвращает процедура
        /// </summary>
        public object Result { get; set; }


        /// <summary>
        /// Это список выходных объектов процедуры и условий моделирования.
        /// Может быть как простым типом, так и массивом.
        /// На triad задается в заголовке в фигурных скобках:
        /// infprocedure ИмяПроцедуры(event ev; in array [10,10] of integer var) {real res} : integer
        /// Key - имя параметра в коде, value - значение переменной
        /// </summary>
        public Dictionary<string, object> OutputResults { get; set; }

        public ProcedureResult(string name)
        {
            ProcedureName = name;
        }
    }
}