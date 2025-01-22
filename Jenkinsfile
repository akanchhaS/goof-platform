pipeline {
  agent any
  stages {
    stage('Checkout Source Code') {
      steps {
        checkout scm
      }
    }

    stage('Install Snyk CLI') {
      steps {
        script {
          sh 'npm install -g snyk'
        }

      }
    }

    stage('Authorize Snyk CLI') {
      steps {
        withCredentials(bindings: [string(credentialsId: 'SNYK_TOKEN', variable: 'SNYK_TOKEN')]) {
          sh 'snyk auth ${SNYK_TOKEN}'
        }

      }
    }

    stage('Build App') {
      steps {
        sh 'echo no-op'
      }
    }

    stage('Build Docker Image') {
      steps {
        script {
          sh 'docker build -t goof-platform-app .'
        }

      }
    }

    stage('Snyk') {
      parallel {
        stage('Snyk Open Source') {
          steps {
            catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
              sh 'snyk test --all-projects --sarif-file-output=results-open-source.sarif'
            }

            recordIssues(tool: sarif(name: 'Snyk Open Source', id: 'snyk-open-source', pattern: 'results-open-source.sarif'))
          }
        }

        stage('Snyk Code') {
          steps {
            catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
              sh 'snyk code test --sarif-file-output=results-code.sarif'
            }

            recordIssues(tool: sarif(name: 'Snyk Code', id: 'snyk-code', pattern: 'results-code.sarif'))
          }
        }

        stage('Snyk Container') {
          steps {
            catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
              sh 'snyk container test goof-platform-app --file=Dockerfile --sarif-file-output=results-container.sarif'
            }

            recordIssues(tool: sarif(name: 'Snyk Container', id: 'snyk-container', pattern: 'results-container.sarif'))
          }
        }

        stage('Snyk IaC') {
          steps {
            catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
              sh 'snyk iac test --sarif-file-output=results-iac.sarif'
            }

            recordIssues(tool: sarif(name: 'Snyk IaC', id: 'snyk-iac', pattern: 'results-iac.sarif'))
          }
        }

      }
    }

  }
  tools {
    nodejs 'NodeJS 18.4.0'
  }
  post {
    always {
      script {
        sh 'docker rmi goof-platform-app || true' // Remove the Docker image after the pipeline
      }

    }

  }
}