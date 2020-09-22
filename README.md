# App-Rastreamento-API
Rest API for App Rastreamento.<br />
App for Tracking Objects sent by "Correios" from Brazil or abroad to Brazil.<br />
<br />
<b>Using</b>:<br />
- .Net Core<br />
- MySQL<br />
- AWS Lambda<br />
- AWS SQS<br />
- AWS CloudWatch<br />
- AWS API Gateway<br />


<b>Class BuscaAtualizacao</b><br />
- Used to search for updates to objects being tracked by app users and to process objects that will be searched for updates to objects being tracked.

<b>Class MyDbContext</b><br />
- Method used to access the database.

<b>Class Util</b><br />
- Class with methods used by other methods, thus creating only one method and reusing it several times.

<b>Folder Controllers</b><br />
- Methods used as API path, where the class name represents the API that will be created.
  
<b>Folder Models</b><br />
- Methods used as models to access the database and be returned in APIs.

  
 
