package tasks;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import tasks.repository.SortingTaskInMemoryRepository;
import tasks.repository.SortingTaskJdbcRepository;
import tasks.repository.SortingTaskRepository;
import tasks.repository.SortingTaskValidator;
import tasks.service.TaskService;
import tasks.utils.ObservableTaskRunner;
import tasks.utils.TaskStack;

import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

/**
 * Created by grigo
 */

@Configuration
public class SortingTasksConfig {
    @Bean
    @Primary
    public Properties jdbsProps(){
        Properties serverProps=new Properties();
        try {
            serverProps.load(getClass().getResourceAsStream("/bd.config"));
            System.out.println("Properties set. ");
            serverProps.list(System.out);
        } catch (IOException e) {
            System.out.println("Cannot find bd.config "+e);

        }

        return  serverProps;

    }
   @Bean
    public SortingTaskRepository createRepository(Properties jdbcProps){

        return new SortingTaskJdbcRepository(jdbcProps);
    }

    @Bean
    @Primary
    public SortingTaskRepository createRepositoryMemo(){

        return new SortingTaskInMemoryRepository(new SortingTaskValidator());
    }

    @Bean(name="taskStack")
    public TaskStack createStack(){
        return new TaskStack();
    }

    @Bean(name="obsRunner")
    public ObservableTaskRunner createRunner(TaskStack stack){
        return new ObservableTaskRunner(stack);
    }

    @Bean(name="taskService")
    public TaskService taskService(SortingTaskRepository repo, ObservableTaskRunner runner){
        return new TaskService(repo,runner);
    }

}
