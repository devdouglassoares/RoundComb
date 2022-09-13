Os métodos que devolvem o XSD, sejam da mensagem, seja do corpo da mesma, devolve sempre o XSD total.

Dado o Serializador utilizado, os atributos marcados como IsRequired=true, irão aparecer no XSD como 
	minOccurs="0"

Os atributos nullable (string, ou outros definidos como tal), irão aparecer no XSD como
    nillable="true" 