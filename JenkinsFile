pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                git credentialsId: 'cdeb0848-5ff1-4eba-8762-0296a3f00398', url: 'https://github.com/basitshah94/multistore-be-api.git'
                echo 'Building the API project';
            }
        }
        stage('Unit-Test') {
            steps {
                echo 'Running JUnit tests'
                
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying to stage environment for more tests'
            }
        }
    }
    
    post{
        always{
            echo 'This will always run'
        }
        success{
            echo 'This will run only if successful'
        }
        failure{
            echo 'This will run only if failed'
        }
        unstable{
            echo 'This will run only if the run was marked as unstable'
        }
        changed{
            echo 'This will only run if the state of pipeline has changed. For example, if the pipeline was previously failing but is now successful'
        }
        
    }
    
}
