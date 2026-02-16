using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;


#region STORED PROCEDURE

/*
 --SQL
CREATE VIEW V_EMPLEADOSV2
AS
	SELECT EMP_NO AS IDEMPLEADO,EMP.APELLIDO,EMP.OFICIO,EMP.SALARIO, DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS NOMBRE, DEPT.LOC AS LOCALIDAD FROM EMP
	INNER JOIN DEPT
	ON EMP.DEPT_NO = DEPT.DEPT_NO
GO
alter procedure SP_ALL_EMPLEADOS
as
	select * from V_EMPLEADOSV2
go
 */

//DELIMITER //

//CREATE PROCEDURE SP_ALL_VEMPLEADOS()
//BEGIN
//    SELECT * FROM V_EMPLEADOSV2;
//END //

//DELIMITER;

#endregion
namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosSQLServer:IRepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleadosSQLServer(EmpleadosContext context)
        {
            this.context = context;
        }
        public async Task<List<VistaEmpleadoDepartamento>> GetEmpleadosDepartamentoAsync()
        {
            //var consulta = from datos in this.context.Empleados
            //               select datos;
            //return await consulta.ToListAsync();
            string sql = "SP_ALL_VEMPLEADOS";
            var consulta = this.context.Empleados
                .FromSqlRaw(sql);
            List<VistaEmpleadoDepartamento> data = await
                consulta.ToListAsync();
            return data;
        }
        public async Task<VistaEmpleadoDepartamento> GetDetallesEmpleadosDepartamentoAsync(int idEmp)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdEmpleado== idEmp
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task InsertEmpleadoAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDept)
        {

                 
            string sql = "SP_CREATE_EMPLEADO @APELLIDO, @OFICIO, @DIR, @SALARIO, @COMISION, @DNOMBRE";
            SqlParameter pApellido = new SqlParameter("@APELLIDO", apellido);
            SqlParameter pOficio = new SqlParameter("@OFICIO", oficio);
            SqlParameter pDir = new SqlParameter("@DIR", dir);
            SqlParameter pSalario = new SqlParameter("@SALARIO", salario);
            SqlParameter pComision = new SqlParameter("@COMISION", comision);
            SqlParameter pNombreDept = new SqlParameter("@DNOMBRE", nombreDept);
            
            await this.context.Database.ExecuteSqlRawAsync(sql, pApellido, pOficio, pDir, pSalario, pComision, pNombreDept);
        }

    }
}
