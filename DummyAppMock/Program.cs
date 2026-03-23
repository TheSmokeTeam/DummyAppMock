var executionBuilder = new QaaS.Mocker.ExecutionBuilder();
var configurator = new DummyAppMock.MockerExecutionBuilderConfigurator();

configurator.Configure(executionBuilder);

new QaaS.Mocker.MockerRunner(executionBuilder).Run();
