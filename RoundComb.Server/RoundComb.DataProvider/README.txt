IBusinesssValidator

Contém as operações que todos os validadores de negócio devem implementar

Se um dado cliente não tiver necessidade da validação, deverá ser codificada como not implemented. Exemplo:
		public ErrorMsgModel validateInterveniente(IntervenientePrincipalExternalModel interveniente, object dependsOn)
        {
            throw new System.NotImplementedException();
        }



Nas classes que implementem IBusinessValidator, o ponto de partida é sempre:

		public ErrorMsgModel validateProcesso(IBusinessData processoToValidate)

		 - este método deve efetuar as validações do processo e invocar as validações do nível seguinte