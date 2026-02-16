using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{   


//#region VISTAS
////    CREATE OR REPLACE VIEW V_EMPLEADOSV2 AS
////SELECT
////    EMP.EMP_NO AS IDEMPLEADO,
////    EMP.APELLIDO AS APELLIDO,
////    EMP.OFICIO AS OFICIO,
////    EMP.SALARIO AS SALARIO,
////    EMP.DEPT_NO AS IDDEPARTAMENTO,
////    DEPT.DNOMBRE AS NOMBRE,
////    DEPT.LOC AS LOCALIDAD
////FROM EMP
////INNER JOIN DEPT
////    ON EMP.DEPT_NO = DEPT.DEPT_NO;


//    //DELIMITER //

////CREATE PROCEDURE SP_CREATE_EMPLEADO(
////    IN p_apellido NVARCHAR(50),
////    IN p_oficio NVARCHAR(50),
////    IN p_dir INT,
////    IN p_salario INT,
////    IN p_comision INT,
////    IN p_dnombre NVARCHAR(150)
////)
////BEGIN
////    DECLARE v_dept_no INT;
////DECLARE v_emp_no INT;

////    -- Obtener ID de departamento
////    SELECT DEPT_NO INTO v_dept_no
////    FROM DEPT
////    WHERE DNOMBRE = p_dnombre
////    LIMIT 1;

////    -- Generar EMP_NO automáticamente
////    SELECT IFNULL(MAX(EMP_NO), 0) + 1 INTO v_emp_no
////    FROM EMP;

////    -- Insertar empleado
////    INSERT INTO EMP(EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO)
////    VALUES(v_emp_no, p_apellido, p_oficio, p_dir, CURDATE(), p_salario, p_comision, v_dept_no);
////END //

////DELIMITER ;

//#endregion



    public class RepositoryEmpleadosMySql : IRepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleadosMySql(EmpleadosContext context)
        {
            this.context = context;
        }
        public async Task<VistaEmpleadoDepartamento> GetDetallesEmpleadosDepartamentoAsync(int idEmp)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdEmpleado == idEmp
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<VistaEmpleadoDepartamento>> GetEmpleadosDepartamentoAsync()
        {
            var consulta = this.context.Empleados.FromSqlRaw("CALL SP_ALL_VEMPLEADOS()");
            return await consulta.ToListAsync();

            //var consulta = from datos in this.context.Empleados
            //               select datos;
            //return await consulta.ToListAsync();
        }

        public async Task InsertEmpleadoAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDept)
        {

        
            string sql = "CALL SP_CREATE_EMPLEADO(@apellido, @oficio, @dir, @salario, @comision, @nombreDept);";

            MySqlParameter pApellido = new MySqlParameter("@apellido", apellido);
            MySqlParameter pOficio = new MySqlParameter("@oficio", oficio);
            MySqlParameter pDir = new MySqlParameter("@dir", dir);
            MySqlParameter pSalario = new MySqlParameter("@salario", salario);
            MySqlParameter pComision = new MySqlParameter("@comision", comision);
            MySqlParameter pNombreDept = new MySqlParameter("@nombreDept", nombreDept);

            await this.context.Database.ExecuteSqlRawAsync(sql, pApellido, pOficio, pDir, pSalario, pComision, pNombreDept);
        

    }

}

}