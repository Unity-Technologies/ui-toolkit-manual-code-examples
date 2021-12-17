FROM adoptopenjdk/openjdk11:alpine as builder
ARG build_jar=true
COPY . .
# Set up wait-for script for docker-compose
COPY wait-for.sh /usr/local/bin/
RUN chmod a+rx /usr/local/bin/wait-for.sh
RUN apk add --update unzip pcre-tools ca-certificates
# Extract gradle version from wrapper and save it in temprorary file for easy access
RUN echo $(cat gradle/wrapper/gradle-wrapper.properties | pcregrep -o1 "gradle-(.*?)-bin") > GRADLE_VERSION
# Fetch, unzip and run gradle while dynamically reading version number from file created above
ENV GRADLE_HOME /opt
RUN mkdir -p $GRADLE_HOME
RUN wget -P $GRADLE_HOME https://services.gradle.org/distributions/gradle-$(cat GRADLE_VERSION)-bin.zip
RUN unzip -q $GRADLE_HOME/gradle-$(cat GRADLE_VERSION)-bin.zip -d $GRADLE_HOME
# Remove gradle version from folder name so we can add it to PATH
RUN mv $GRADLE_HOME/gradle-$(cat GRADLE_VERSION) $GRADLE_HOME/gradle
ENV PATH $PATH:$GRADLE_HOME/gradle/bin
RUN if [ "$build_jar" = true ] ; then gradle shadowJar ; fi

LABEL maintainer="Unity3D"

FROM adoptopenjdk/openjdk11:alpine-jre
# Install dependecies of secret fetcher
RUN apk add --update curl jq
ENV APP_DIR /ads-kotlin-service-template
RUN mkdir $APP_DIR
COPY --from=builder /build/libs/ads-kotlin-service-template.jar $APP_DIR
CMD java -XX:MaxRAMPercentage=90 -jar $APP_DIR/ads-kotlin-service-template.jar
